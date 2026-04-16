# Phase 18 Hygiene Note

## Added Ignore Rules
- `orchestrator/state/CURRENT_HANDOFF.md`
- `orchestrator/state/SESSION_STATE.md`
- `orchestrator/tasks/backlog.json`

## Why
- These files change almost every run/session and create noisy commits.
- They are runtime-operational state, not stable source artifacts.
- Ignoring them keeps suite repo commits focused on code and durable docs.

## Index Cleanup
- Tracked entries for these files were removed from git index with `git rm --cached`.
- Local copies remain in workspace and continue to be used by orchestrator.
