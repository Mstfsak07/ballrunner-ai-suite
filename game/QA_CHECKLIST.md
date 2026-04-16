# QA Risk Checklist (P2-T005)

## Critical Risks

### 1) Ball count desync
- [ ] Gate gecisinden sonra UI ball count ile BallGroupController.CurrentCount ayni mi?
- [ ] Obstacle hasarindan sonra negatif sayiya dusme engelleniyor mu?
- [ ] Multiply gate sonrasi max cap (80) uygulanmis mi?
- [ ] Finish aninda hesaplanan kalan top ile ekrandaki deger ayni mi?

### 2) Gate duplicate trigger
- [ ] Her gate tek geciste yalnizca bir kere uygulanıyor mu?
- [ ] Yavas hizda collider icinde kalinca ikinci kez tetiklenmiyor mu?
- [ ] Ayni frame icinde coklu collider temasinda yine tek uygulama mi?

### 3) Revive state bugs
- [ ] Revive bir run'da en fazla bir kez kullaniliyor mu?
- [ ] Revive sonrasi state `Playing`e donuyor mu?
- [ ] Revive sonrasi skor/coin iki kez yazilmiyor mu?

### 4) Finish result integrity
- [ ] Win/Fail karari sadece kalan top > 0 kuraliyla mi veriliyor?
- [ ] Coin odulu bir kez mi ekleniyor?
- [ ] Result paneldeki score/coin, RunResultService son degerleriyle eslesiyor mu?

## Functional Test Matrix

### Movement
- [ ] Auto-forward her cihazda stabil hizda calisiyor.
- [ ] Drag clamp sinirlari disina cikilmiyor.

### Gates
- [ ] +N / -N / x2 her biri beklenen sonucu veriyor.
- [ ] Kapilarin gorsel labellari dogru degeri gosteriyor.

### Obstacles
- [ ] SpinBar temasinda hasar uygulanir.
- [ ] DamageWall tek geciste bir kez hasar uygular.

### Economy
- [ ] Level odulu coin'e eklenir.
- [ ] Start Balls upgrade maliyetleri 50/100/200/350/500 uyumlu.
- [ ] Coin Bonus upgrade odul carpani dogru uygular.
- [ ] Save/load sonrasi coin ve upgrade seviyeleri korunur.

## Performance
- [ ] 10 dk testte crash yok.
- [ ] FPS orta segment Android'de hedefe yakin (60 fps).
- [ ] Pool acikken GC spike minimum.

## Pre-Release Checks
- [ ] Build Settings sahne sirasi: Boot -> MainMenu -> Gameplay.
- [ ] Portrait lock dogru ayarlandi.
- [ ] Ilk acilis akisi menuye duzgun dusuyor.
- [ ] Fail/Win panel butonlari calisiyor.

## Known Blockers (Current)
- Gemini CLI headless invocation currently hangs in this environment.
- Claude invocation returned environment resource warning before producing checklist.
- Fallback deliverable produced locally by Codex.
