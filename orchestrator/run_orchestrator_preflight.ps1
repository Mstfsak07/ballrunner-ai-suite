param(
  [switch]$CleanupStale,
  [int]$SmokeTimeoutSec = 30,
  [int]$AnalyticsLimit = 30,
  [int]$CompactLimit = 30
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path

Write-Host "[1/4] Checking prerequisites"
powershell -ExecutionPolicy Bypass -File "$root\check_prereqs.ps1"
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "[2/4] Validating agent stack"
$validateArgs = @("-ExecutionPolicy", "Bypass", "-File", "$root\validate_agent_stack.ps1", "-SmokeTimeoutSec", "$SmokeTimeoutSec")
if ($CleanupStale) { $validateArgs += "-CleanupStale" }
powershell @validateArgs
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "[3/4] Generating dispatch analytics"
python "$root\analyze_dispatch_runs.py" --limit $AnalyticsLimit
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "[4/4] Exporting compact queue"
python "$root\export_compact_queue.py" --limit $CompactLimit --include-assigned
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "Orchestrator preflight PASS"
