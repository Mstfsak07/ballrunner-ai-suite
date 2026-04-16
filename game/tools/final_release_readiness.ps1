param(
  [switch]$RunPreflight,
  [string]$VersionName,
  [int]$VersionCode
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path | Split-Path -Parent
$reportPath = Join-Path $root "FINAL_RELEASE_READINESS.md"

$autoChecks = New-Object 'System.Collections.Generic.List[string]'
$failed = $false

function Run-Check($name, [scriptblock]$action) {
  try {
    & $action | Out-Null
    $script:autoChecks.Add("- [OK] $name")
  }
  catch {
    $script:autoChecks.Add("- [FAIL] $name :: $($_.Exception.Message)")
    $script:failed = $true
  }
}

if ($RunPreflight) {
  if ([string]::IsNullOrWhiteSpace($VersionName) -or $VersionCode -le 0) {
    throw "When -RunPreflight is used, VersionName and VersionCode are required."
  }
  Run-Check "Preflight pipeline" { & "$root\tools\run_preflight.ps1" -VersionName $VersionName -VersionCode $VersionCode }
}

Run-Check "Internal readiness strict" { & "$root\tools\verify_internal_readiness.ps1" -Strict }
Run-Check "Release manifest generation" { & "$root\tools\generate_release_manifest.ps1" }
Run-Check "Ops daily report generation" { & "$root\tools\generate_ops_daily_report.ps1" }
Run-Check "Ads SDK bridge stub exists" {
  $adsBridge = Join-Path $root "Assets\Scripts\Ads\AdsSdkBridgeStub.cs"
  if (-not (Test-Path $adsBridge)) { throw "AdsSdkBridgeStub.cs missing" }
}

$manualGates = @(
  "Unity Editor scene/prefab reference validation",
  "Real device Android/iOS build smoke test",
  "Real ad SDK account + placement integration verification",
  "Store listing assets and policy checklist"
)

$overall = if ($failed) { "NOT_READY" } else { "AUTO_READY_MANUAL_GATES_PENDING" }

$lines = New-Object 'System.Collections.Generic.List[string]'
$lines.Add("# Final Release Readiness")
$lines.Add("")
$lines.Add("Generated: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')")
$lines.Add("Overall: $overall")
$lines.Add("")
$lines.Add("## Automated Checks")
foreach ($c in $autoChecks) {
  $lines.Add($c)
}
$lines.Add("")
$lines.Add("## Manual Gates (Required)")
foreach ($g in $manualGates) {
  $lines.Add("- [ ] $g")
}
$lines.Add("")

Set-Content -Path $reportPath -Value ($lines -join "`n") -Encoding UTF8
Write-Host "Final readiness report: $reportPath"

if ($failed) { exit 2 }
