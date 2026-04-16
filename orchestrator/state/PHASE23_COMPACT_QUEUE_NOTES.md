# Phase 23 Compact Queue Notes

- New script: `export_compact_queue.py`
- Purpose: creates low-token JSON queue from backlog for routing and quick context handoff.
- Default mode: pending tasks only.
- Optional mode: include assigned tasks.

## Commands
- `python export_compact_queue.py --limit 25`
- `python export_compact_queue.py --limit 40 --include-assigned`
- `./run_orchestrator.ps1 -ExportCompactQueue -CompactLimit 30 -CompactIncludeAssigned`
