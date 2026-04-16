param(
  [string]$CommitMessage = "chore: automated suite sync",
  [switch]$DryRun
)

$ErrorActionPreference = "Stop"
$tools = Split-Path -Parent $MyInvocation.MyCommand.Path

function Run-Step($name, [scriptblock]$action) {
  Write-Host "==> $name"
  & $action
}

Run-Step "Sync suite repo" { & "$tools\sync_to_suite_repo.ps1" }
Run-Step "Suite pre-commit check" { & "$tools\suite_precommit_check.ps1" }

if ($DryRun) {
  Write-Host "Dry-run complete. Commit/push skipped."
  exit 0
}

Run-Step "Auto commit" { & "$tools\suite_auto_commit.ps1" -Message $CommitMessage }
Run-Step "Auto push" { & "$tools\suite_auto_push.ps1" -Branch main }

Write-Host "Suite release flow completed."
