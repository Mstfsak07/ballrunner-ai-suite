# Phase 14 Handoff Notes

## Preflight Pipeline
- `tools/run_preflight.ps1 -VersionName <name> -VersionCode <code>`

Pipeline order:
1. Version bump
2. Readiness strict
3. Release manifest
4. Ops daily report
5. Snapshot export
6. Snapshot cleanup

## Additional Outputs
- `PREFLIGHT_RESULT.md`
- `PREFLIGHT_ARTIFACT_SUMMARY.md`

## Troubleshooting
- If readiness fails, fix missing docs and rerun preflight.
- If manifest JSON invalid, regenerate manifest before snapshot.
- If snapshot cleanup removed needed archive, rerun export immediately.
