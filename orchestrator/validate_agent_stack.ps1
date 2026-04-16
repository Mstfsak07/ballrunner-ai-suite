param(
  [switch]$CleanupStale,
  [int]$SmokeTimeoutSec = 30
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$runsDir = Join-Path $root "runs"
New-Item -ItemType Directory -Force -Path $runsDir | Out-Null
$stamp = Get-Date -Format "yyyyMMdd-HHmmss"
$report = Join-Path $runsDir "agent-stack-$stamp.md"

$healthCmd = if ($CleanupStale) {
  "powershell -ExecutionPolicy Bypass -File `"$root\cli_healthcheck.ps1`" -CleanupStale"
} else {
  "powershell -ExecutionPolicy Bypass -File `"$root\cli_healthcheck.ps1`""
}

$smokeCmd = "powershell -ExecutionPolicy Bypass -File `"$root\dispatch_smoke_test.ps1`" -TimeoutSec $SmokeTimeoutSec"

$healthOut = cmd /c $healthCmd 2>&1
$healthCode = $LASTEXITCODE
$smokeOut = cmd /c $smokeCmd 2>&1
$smokeCode = $LASTEXITCODE

$overall = if ($healthCode -eq 0 -and $smokeCode -eq 0) { "PASS" } else { "FAIL" }

$lines = @(
  "# Agent Stack Validation",
  "",
  "Timestamp: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')",
  "Overall: $overall",
  "Healthcheck Exit: $healthCode",
  "Smoke Exit: $smokeCode",
  "",
  "## Healthcheck Output",
  '```text'
)
$lines += $healthOut
$lines += @('```', '', '## Smoke Output', '```text')
$lines += $smokeOut
$lines += @('```', '')

Set-Content -Path $report -Value $lines -Encoding UTF8
Write-Host "Validation report: $report"

if ($overall -ne "PASS") { exit 1 }
