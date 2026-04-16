param(
  [string]$RepoPath = "C:\Users\akMuratNET\Desktop\ballrunner-ai-suite",
  [string]$Message = "chore: sync latest BallRunner game and orchestrator updates"
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $RepoPath)) { throw "Repo not found: $RepoPath" }
$inside = git -C $RepoPath rev-parse --is-inside-work-tree 2>$null
if ($inside -ne "true") { throw "Not a git repo: $RepoPath" }

git -C $RepoPath add .
$staged = git -C $RepoPath diff --cached --name-only
if ([string]::IsNullOrWhiteSpace($staged)) {
  Write-Host "No staged changes."
  exit 0
}

git -C $RepoPath commit -m $Message
Write-Host "Commit created."
