# Phase 15 Usage Notes

## Suite Sync
```powershell
.\tools\sync_to_suite_repo.ps1
```
Syncs:
- `BallRunnerHybrid` -> `ballrunner-ai-suite/game`
- `ai-game-orchestrator` -> `ballrunner-ai-suite/orchestrator`

## Suite Pre-commit Check
```powershell
.\tools\suite_precommit_check.ps1
```
Shows:
- active branch
- remote URLs
- short git status

## Recommended Flow
1. Run suite sync
2. Run suite pre-commit check
3. Commit in `ballrunner-ai-suite`
4. Push to GitHub
