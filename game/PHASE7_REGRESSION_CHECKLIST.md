# Phase 7 Regression Checklist

## A) Level Loop (P7-T001)
- [ ] L1: Level tamamlaninca bir sonraki levele gecebilir.
- [ ] L2: Son level sonrasi index catalog basina doner (wrap).
- [ ] L3: Restart/replay ayni leveli tekrar yukler.
- [ ] L4: Oyun yeniden acildiginda level index persist edilmis olur.
- [ ] L5: Catalog bosken crash olmaz, warning log duser.
- [ ] L6: BuildCurrentLevel bir run icinde duplicate spawn uretmez.

## B) Tutorial Hand (P7-T002)
- [ ] T1: Ilk run'da hand gorunur.
- [ ] T2: Ilk drag inputunda hand aninda gizlenir.
- [ ] T3: Sonraki runlarda hand tekrar acilmaz (persist).
- [ ] T4: PlayerMover null oldugunda script crash vermez.
- [ ] T5: Scene reload sonrasi tutorial state korunur.
- [ ] T6: Input race durumunda hand iki kez toggle olmaz.

## C) Revive Flow (P5-T001 + P6-T002)
- [ ] R1: Fail panelde revive bir runda sadece 1 kez kullanilir.
- [ ] R2: Revive ile menuye donmeden ayni run devam eder.
- [ ] R3: Revive sonrasi panel kapanir, player hareketi aktif olur.
- [ ] R4: Revive sonrasi HUD coin/progress force refresh olur.
- [ ] R5: Revive coin/score duplicate odul uretmez.
- [ ] R6: Revive state gecisleri (Fail->Revive->Playing) tutarlidir.
- [ ] R7: Revive sonrasi finish sonucu normal akista tamamlanir.

## D) Rewarded x2 (P6-T001)
- [ ] W1: Win panelde x2 butonu gorunur.
- [ ] W2: Ilk tikta odul verilir, buton kilitlenir.
- [ ] W3: Ikinci tik duplicate odul uretmez.
- [ ] W4: Reward callback gecikse bile one-shot guard korunur.
- [ ] W5: Yeni run basladiginda claim guard sifirlanir.
- [ ] W6: HUD coin degeri odul sonrasi anlik guncellenir.

## Priority Order
1. R1 (revive tek hak)
2. W2 (x2 one-shot)
3. L2 (level wrap)
4. T2/T3 (tutorial hide + persist)
5. L4 (level progress persist)
