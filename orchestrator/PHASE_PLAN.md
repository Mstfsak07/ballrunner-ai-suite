# Ball Runner Project Plan (Reworked)

## Product Direction
- Genre: Hyper-casual core + hybrid-casual extension
- Platform: Unity, Android-first, portrait
- Timeline: 7 days to internal test build
- Team mode: Solo developer + AI orchestrated execution

## Scope Corrections
- Keep MVP strict: no skin economy, no daily quests in week-1
- Monetization in MVP: interstitial + rewarded x2 + revive only
- Max visible balls in MVP: 80 hard cap
- Mandatory anti-bug checks from day 1: gate double trigger, count desync, revive state

## Phase Breakdown

### Phase 0 - Foundation (Half day)
Goal: Production-safe project skeleton
Deliverables:
- Unity folder + scene bootstrap (Boot/MainMenu/Gameplay)
- Core game state machine (Boot, Menu, Playing, Win, Fail, Revive)
- Config ScriptableObjects (GameConfig, EconomyConfig)
- Logging + debug toggles
Done when:
- Empty gameplay scene runs with state transitions without errors

### Phase 1 - Core Playable (Day 1-2)
Goal: One full playable loop
Deliverables:
- Auto-forward movement + drag left-right control
- BallGroupController (spawn/despawn/count sync)
- Gates: +N / -N / x2
- Obstacles: SpinBar + DamageWall
- Finish flow with score conversion
Done when:
- One level starts and ends reliably in 20-40s

### Phase 2 - MVP Content (Day 3-5)
Goal: 10-level playable MVP
Deliverables:
- LevelData pipeline + 10 levels
- HUD + Win/Fail panels + restart/next
- Coin earn flow + 2 upgrades (Start Balls, Coin Bonus)
- Object pooling for balls and gate fx
Done when:
- 10-minute stress session without crash/desync

### Phase 3 - Monetization + Launch Prep (Day 6-7)
Goal: Internal test release candidate
Deliverables:
- Ads integration hooks
- Interstitial frequency control (every 2-3 fails)
- Rewarded x2 coin + one-time revive rewarded
- Tutorial hand + vibration + basic juice polish
- Internal test assets (icon/screenshot checklist)
Done when:
- Android internal test build uploaded

## Agent Work Partition

### Codex (gpt-5.4)
- All gameplay systems, pooling, save/economy implementation
- Integration and refactor tasks

### Claude Sonnet 4.6
- QA checklist, risk matrix, test scenarios, launch checklist

### Claude Opus 4.6
- Architecture review, critical edge cases, failure-mode analysis

### Gemini 3 Pro / 3 Flash
- Level pattern generation, onboarding flow variants, short-form design prompts
- `gemini-3-flash` for quick draft tasks
- `gemini-3-pro` for larger content batches

## Hard Quality Gates
- Ball count is single source of truth in BallGroupController
- Gate trigger must be idempotent (one gate, one application)
- Revive can be consumed once and restores deterministic state
- Pooling required before 10-level test

## Immediate Start Order (Now)
1. Implement Phase 0 architecture skeleton
2. Implement Phase 1 movement + gate pipeline
3. Validate with playable test level
