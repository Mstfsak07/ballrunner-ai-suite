param(
  [int]$KeepLatest = 5
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path | Split-Path -Parent
$snapRoot = Join-Path $root "release-snapshots"

if (-not (Test-Path $snapRoot)) {
  Write-Host "No snapshot directory found."
  exit 0
}

$dirs = Get-ChildItem $snapRoot -Directory | Sort-Object LastWriteTime -Descending
$remove = $dirs | Select-Object -Skip $KeepLatest

foreach ($d in $remove) {
  Remove-Item -Recurse -Force $d.FullName
  Write-Host "Removed: $($d.Name)"
}

Write-Host "Cleanup complete. Kept latest $KeepLatest snapshots."
