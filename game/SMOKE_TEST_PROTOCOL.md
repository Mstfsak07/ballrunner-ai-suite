# Playable Smoke Test Protocol (P4-T003)

Duration: 5-7 minutes
Scope: Editor play-mode sanity pass

## A) Boot and Scene Flow (1 min)
- [ ] Play tusunda uygulama Boot -> MainMenu akisinda aciliyor.
- [ ] Play butonuyla Gameplay sahnesi aciliyor.
- [ ] Crash veya missing reference hatasi yok.

## B) Movement and Ball Core (1 min)
- [ ] Player otomatik ileri gidiyor.
- [ ] Drag ile yatay hareket clamp sinirlarinda kaliyor.
- [ ] Baslangic top sayisi `GameplayBootstrapper` ile set ediliyor.

## C) Runtime Level Build (1 min)
- [ ] LevelData'dan gate spawn oluyor.
- [ ] LevelData'dan obstacle spawn oluyor.
- [ ] Yanlis/missing prefab durumunda warning geliyor, oyun patlamiyor.

## D) Gate/Obstacle/Finish (2 min)
- [ ] `+N` gate top sayisini arttiriyor.
- [ ] `-N` gate top sayisini azaltiyor.
- [ ] `x2` gate sayiyi carpiyor ve max cap'e uyuyor.
- [ ] Obstacle hasari count'tan dusuyor.
- [ ] Finish'te score/coin hesaplanip Win/Fail state tetikleniyor.

## E) Economy + Ads Hook (1-2 min)
- [ ] Coin odulu kaydediliyor (save/load).
- [ ] Upgrade (Start Balls / Coin Bonus) harcama ve seviye artisina izin veriyor.
- [ ] Fail akisinda interstitial cadence mock log cikiyor.
- [ ] Rewarded coin/revive eventleri mock akista tetikleniyor.

## Exit Criteria
- [ ] Yukaridaki maddelerin en az %90'i sorunsuz.
- [ ] Critical blocker yok: count desync, gate duplicate trigger, crash.

## If Failed
- [ ] Repro adimlarini yaz.
- [ ] Hangi scriptte oldugunu not et.
- [ ] Ilk oncelik: crash > desync > gameplay bug > polish.
