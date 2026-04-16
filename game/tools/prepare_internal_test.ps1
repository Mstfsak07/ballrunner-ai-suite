param(
  [string]$VersionName = "0.1.0-internal",
  [int]$VersionCode = 1,
  [string]$OrchestratorBacklog = "C:\Users\akMuratNET\ai-game-orchestrator\tasks\backlog.json"
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path | Split-Path -Parent
$release = Join-Path $root "RELEASE_NOTES.md"

$required = @(
  "LAUNCH_CHECKLIST.md",
  "PHASE7_REGRESSION_CHECKLIST.md",
  "PHASE9_WIRING_NOTES.md"
)

$missing = @()
foreach ($f in $required) {
  if (-not (Test-Path (Join-Path $root $f))) { $missing += $f }
}

if (-not (Test-Path $OrchestratorBacklog)) {
  throw "Backlog file not found: $OrchestratorBacklog"
}

$backlog = Get-Content -Raw $OrchestratorBacklog | ConvertFrom-Json
$completed = @($backlog.tasks | Where-Object { $_.status -eq "completed" } | Sort-Object priority)

$lines = @()
$lines += "# Internal Test Release Notes"
$lines += ""
$lines += "Date: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
$lines += "VersionName: $VersionName"
$lines += "VersionCode: $VersionCode"
$lines += "Project: $($backlog.project)"
$lines += ""
$lines += "## Completed Tasks"

foreach ($t in $completed) {
  $lines += "- [$($t.id)] $($t.title)"
}

if ($missing.Count -gt 0) {
  $lines += ""
  $lines += "## Missing Required Files"
  foreach ($m in $missing) { $lines += "- $m" }
}

$lines -join "`n" | Set-Content -Encoding UTF8 $release

Write-Host "Release notes written: $release"
if ($missing.Count -gt 0) {
  Write-Host "Missing files:"; $missing | ForEach-Object { Write-Host " - $_" }
  exit 2
}

Write-Host "Internal test prep check passed."
