# Phase 27 Ads Integration Note

## What changed
- `AdsManager` now exposes request events for SDK calls:
  - `OnInterstitialRequested`
  - `OnRewardedCoinRequested`
  - `OnRewardedReviveRequested`
- Rewarded completion is now explicit via callbacks:
  - `ResolveRewardedCoin(bool granted)`
  - `ResolveRewardedRevive(bool granted)`
- Added `AdsSdkBridgeStub` as a wiring example.

## Integration contract
1. Keep `useMockAds = false` on `AdsManager` for SDK mode.
2. Add `AdsSdkBridgeStub` (or your real bridge component) on same object.
3. In bridge, replace SDK hook blocks with SDK show calls.
4. On SDK callback, call `ResolveRewardedCoin(true/false)` or `ResolveRewardedRevive(true/false)`.

## Safety
- Duplicate rewarded requests are guarded.
- Revive is still one-time per run.
