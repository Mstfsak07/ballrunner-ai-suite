param(
  [string]$RepoPath = "C:\Users\akMuratNET\Desktop\ballrunner-ai-suite"
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $RepoPath)) { throw "Repo not found: $RepoPath" }

$inside = git -C $RepoPath rev-parse --is-inside-work-tree 2>$null
if ($inside -ne "true") { throw "Not a git repo: $RepoPath" }

$branch = git -C $RepoPath rev-parse --abbrev-ref HEAD
$remote = git -C $RepoPath remote -v
$status = git -C $RepoPath status --short
$volatileTracked = @(
  "orchestrator/state/CURRENT_HANDOFF.md",
  "orchestrator/state/SESSION_STATE.md",
  "orchestrator/tasks/backlog.json"
)
$trackedViolations = @()
foreach ($path in $volatileTracked) {
  $trackedPath = (git -C $RepoPath ls-files $path | Select-Object -First 1)
  if (-not [string]::IsNullOrWhiteSpace($trackedPath)) { $trackedViolations += $path }
}

Write-Host "Repo: $RepoPath"
Write-Host "Branch: $branch"
Write-Host "Remotes:"
Write-Host $remote
Write-Host "Status:"
if ([string]::IsNullOrWhiteSpace($status)) {
  Write-Host "clean"
} else {
  Write-Host $status
}

if ($trackedViolations.Count -gt 0) {
  Write-Error ("Tracked volatile files detected: " + ($trackedViolations -join ", "))
  exit 1
}
