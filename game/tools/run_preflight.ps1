param(
  [Parameter(Mandatory=$true)][string]$VersionName,
  [Parameter(Mandatory=$true)][int]$VersionCode,
  [int]$KeepLatestSnapshots = 5
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path | Split-Path -Parent
$log = Join-Path $root "PREFLIGHT_RESULT.md"

$steps = @()
function Step($name, [scriptblock]$action) {
  try {
    & $action
    $steps += "- [OK] $name"
  } catch {
    $steps += "- [FAIL] $name :: $($_.Exception.Message)"
    throw
  }
}

try {
  Step "Version bump" { & "$root\tools\bump_version.ps1" -VersionName $VersionName -VersionCode $VersionCode | Out-Null }
  Step "Readiness strict" { & "$root\tools\verify_internal_readiness.ps1" -Strict | Out-Null }
  Step "Generate release manifest" { & "$root\tools\generate_release_manifest.ps1" | Out-Null }
  Step "Generate ops daily report" { & "$root\tools\generate_ops_daily_report.ps1" | Out-Null }
  Step "Export release snapshot" { & "$root\tools\export_release_snapshot.ps1" | Out-Null }
  Step "Cleanup old snapshots" { & "$root\tools\clean_release_snapshots.ps1" -KeepLatest $KeepLatestSnapshots | Out-Null }
}
catch {
  # continue to write report
}

$lines = @()
$lines += "# Preflight Result"
$lines += ""
$lines += "Generated: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
$lines += "VersionName: $VersionName"
$lines += "VersionCode: $VersionCode"
$lines += ""
$lines += "## Steps"
$lines += $steps

$lines -join "`n" | Set-Content -Encoding UTF8 $log
Write-Host "Preflight summary: $log"
