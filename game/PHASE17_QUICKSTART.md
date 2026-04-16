# Phase 17 Quickstart

Single command release flow:

```powershell
.\tools\suite_release_flow.ps1 -CommitMessage "chore: sync latest"
```

Dry-run (no commit/push):

```powershell
.\tools\suite_release_flow.ps1 -DryRun
```

Flow steps:
1. sync_to_suite_repo
2. suite_precommit_check
3. suite_auto_commit
4. suite_auto_push
