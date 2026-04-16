param(
  [string]$RepoPath = "C:\Users\akMuratNET\Desktop\ballrunner-ai-suite",
  [string]$Branch = "main"
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $RepoPath)) { throw "Repo not found: $RepoPath" }
$inside = git -C $RepoPath rev-parse --is-inside-work-tree 2>$null
if ($inside -ne "true") { throw "Not a git repo: $RepoPath" }

git -C $RepoPath push origin $Branch
Write-Host "Push completed: origin/$Branch"
