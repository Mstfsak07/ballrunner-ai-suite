# ACCOUNT SWITCH: ONCE THIS ACCOUNT OPENS, READ THIS FILE FIRST

## 1) Source of truth
- Current project state: `state/SESSION_STATE.md`
- Current active handoff: `state/CURRENT_HANDOFF.md`
- Task backlog: `tasks/backlog.json`

## 2) Hard rules
- Never start a new task before reading `SESSION_STATE.md`.
- Never change task status manually outside orchestrator unless emergency.
- If you are Gemini/Claude/Codex account owner, only execute the task assigned in `CURRENT_HANDOFF.md`.
- If anything is ambiguous, stop and add a note to `SESSION_STATE.md` under `Open Questions`.

## 3) Handoff checklist
- Read `SESSION_STATE.md` completely.
- Confirm active task id and owner.
- Confirm expected output file/location.
- Write a short "I understood" note in `SESSION_STATE.md` with timestamp.

## 4) Account identity marker
When you start, write this line in your first response:
`ACCOUNT-CHECK: <agent-name> | task=<TASK-ID> | model=<MODEL>`

This file prevents confusion after account switching.
