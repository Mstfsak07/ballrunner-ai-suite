# Launch Checklist (P3-T002)

## Build Configuration
- [ ] Platform: Android
- [ ] Orientation: Portrait only
- [ ] Scripting backend: IL2CPP
- [ ] Target API level: latest stable
- [ ] Development Build kapali
- [ ] Strip Engine Code aktif

## Scene and Boot Flow
- [ ] Build settings scene order: Boot -> MainMenu -> Gameplay
- [ ] App acilisinda MainMenu'ye dusuyor
- [ ] Gameplay start/restart akisi stabil

## Gameplay Sanity
- [ ] Gate +N/-N/x2 beklenen sonuc veriyor
- [ ] Obstacles hasari count ile tutarli
- [ ] Finish skor/coin donusumu dogru
- [ ] Win/Fail panel akisi calisiyor

## Economy and Save
- [ ] Coin earn flow calisiyor
- [ ] Start Balls upgrade calisiyor
- [ ] Coin Bonus upgrade calisiyor
- [ ] Save/load sonrasi veri korunuyor

## Ads Readiness
- [ ] Interstitial fail cadence (2-3 failde bir) dogrulandi
- [ ] Rewarded x2 coin callback dogrulandi
- [ ] Revive rewarded tek kullanim dogrulandi
- [ ] Mock moddan SDK moda gecis kontrol edildi

## Performance and Stability
- [ ] 10 dakika stress test crashsiz
- [ ] FPS hedefi orta segment cihazda kabul edilebilir
- [ ] GC spike ciddi degil (pool aktif)

## Store Prep (Internal Test)
- [ ] App icon hazir
- [ ] 2-4 screenshot hazir
- [ ] Kisa aciklama hazir
- [ ] Package name final
- [ ] VersionCode/VersionName guncel

## Internal Test Exit Criteria
- [ ] P0-P3 backlog tamamlandi
- [ ] Critical bug yok (count desync, gate double trigger, revive bug)
- [ ] Signed AAB/APK alindi
- [ ] Google Play Internal Testing upload tamam
