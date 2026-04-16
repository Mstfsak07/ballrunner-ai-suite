# Ops Handoff Notes

## Project
- Name: BallRunnerHybrid
- Target: Android internal test
- Current status: Phase 11 completed

## Key Run Commands
- `tools\prepare_internal_test.ps1 -VersionName "0.1.0-internal" -VersionCode 1`
- `tools\generate_release_manifest.ps1`
- `tools\verify_internal_readiness.ps1 -Strict`

## Expected Artifacts
- `RELEASE_NOTES.md`
- `RELEASE_MANIFEST.json`
- `LAUNCH_CHECKLIST.md`
- `SMOKE_TEST_PROTOCOL.md`

## Quick Recovery
- Reset all gameplay/test state: use `DebugProgressTools` context menu or `F6` (DebugHotkeys)
- Validate scene links: run `SceneReferenceValidator`

## Known Constraints
- Claude dispatch can timeout while still producing partial stdout in dispatch logs.
- Gemini headless requires clean temp workspace health-check mode.

## Next Operator Checklist
1. Open Unity scene and run scene validator.
2. Execute strict readiness script.
3. Re-generate manifest/release notes before build upload.
4. Run smoke protocol and update checklist timestamps.
