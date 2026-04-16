# Phase 19 Usage Notes

## What changed
- `tools/suite_precommit_check.ps1` now fails if volatile orchestrator files are tracked in suite git index.
- `tools/sync_to_suite_repo.ps1` now removes volatile orchestrator runtime files after sync.

## Guarded volatile files
- `orchestrator/state/CURRENT_HANDOFF.md`
- `orchestrator/state/SESSION_STATE.md`
- `orchestrator/tasks/backlog.json`

## Expected workflow
1. Run `tools/sync_to_suite_repo.ps1`.
2. Run `tools/suite_precommit_check.ps1`.
3. If precheck fails, untrack violating files (`git rm --cached ...`) and retry.
