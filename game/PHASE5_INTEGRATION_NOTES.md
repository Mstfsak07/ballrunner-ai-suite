# Phase 5 Integration Notes

## Scope
- Revive in-game resume flow
- Rewarded x2 coin hook
- Live HUD coin/progress binding

## Current Status
- Revive flow: implemented (resume without scene reload)
- HUD coin/progress binding: implemented (event + runtime tracker)
- Rewarded x2: hook exists, but idempotency guard should be added

## Quick Verification Checklist
- [ ] Fail panelde `Revive` butonuna basinca panel kapanir ve run devam eder.
- [ ] Bir run icinde revive sadece 1 kez kullanilabilir.
- [ ] Win panelde `x2` butonu bir kez odul verir.
- [ ] Coin HUD degeri odul ve harcama sonrasi anlik guncellenir.
- [ ] Progress slider fail/revive sonrasi tutarli ilerler.

## Risks and Follow-up
1. Rewarded x2 duplicate claim riski
   - Fix: `rewardClaimed` flag ile butonu one-shot yap.
2. Revive sonrasi HUD refresh gecikmesi riski
   - Fix: revive callback sonunda hud refresh tetikle.

## Suggested Next Tasks
- P6-T001: Rewarded x2 one-shot/idempotency guard
- P6-T002: Revive sonrası forced HUD refresh + mini regression pass
