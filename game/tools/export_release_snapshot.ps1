param(
  [string]$SnapshotName
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path | Split-Path -Parent
$ts = Get-Date -Format "yyyyMMdd-HHmmss"
if ([string]::IsNullOrWhiteSpace($SnapshotName)) {
  $SnapshotName = "snapshot-$ts"
}

$target = Join-Path $root ("release-snapshots\" + $SnapshotName)
New-Item -ItemType Directory -Force -Path $target | Out-Null

$items = @(
  "RELEASE_NOTES.md",
  "RELEASE_MANIFEST.json",
  "LAUNCH_CHECKLIST.md",
  "SMOKE_TEST_PROTOCOL.md",
  "PHASE7_REGRESSION_CHECKLIST.md",
  "PHASE9_WIRING_NOTES.md",
  "PHASE8_SETUP_NOTES.md",
  "OPS_HANDOFF.md"
)

foreach ($item in $items) {
  $src = Join-Path $root $item
  if (Test-Path $src) {
    Copy-Item -Force $src (Join-Path $target ([IO.Path]::GetFileName($src)))
  }
}

Write-Host "Snapshot created: $target"
