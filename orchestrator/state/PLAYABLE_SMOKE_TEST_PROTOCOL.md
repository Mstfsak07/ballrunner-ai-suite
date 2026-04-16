# Ball Runner — Playable Smoke Test Protokolü

**Görev:** P4-T003  
**Sürüm:** Phase 4 — Integration Complete  
**Tahmini Süre:** ~5 dakika (editörde)  
**Yürüten:** QA / Geliştirici  

---

## Ön Koşullar

- Unity Editor açık, `BallRunnerHybrid` projesi yüklenmiş
- `Boot` sahnesi aktif (Play bu sahneden başlamalı)
- Console'da önceki oturuma ait hata temizlenmiş (Clear on Play açık)
- `GameConfig` ve `EconomyConfig` ScriptableObject'leri Inspector'da dolu

---

## ADIM 1 — Boot → MainMenu Geçişi (0:00–0:30)

| # | Aksiyon | Beklenen Çıktı | Hata mı? |
|---|---------|----------------|----------|
| 1.1 | Play'e bas | Konsol temiz (exception yok), `Boot` sahnesi yükleniyor | |
| 1.2 | ~1 saniye bekle | Otomatik olarak `MainMenu` sahnesine geçiş | |
| 1.3 | MainMenu UI'ı kontrol et | Play butonu görünür, coin sayacı 0 (ya da kayıtlı değer) | |

**Geçme kriteri:** Boot→MainMenu geçişi exception olmadan tamamlanır.

---

## ADIM 2 — Gameplay Başlangıcı (0:30–1:30)

| # | Aksiyon | Beklenen Çıktı | Hata mı? |
|---|---------|----------------|----------|
| 2.1 | Play butonuna bas | `Gameplay` sahnesi yükleniyor | |
| 2.2 | Sahne yüklenince bekle | `GameplayBootstrapper` Level 1'i yükler ve `LevelRuntimeBuilder` gate + obstacle'ları spawn eder | |
| 2.3 | HUD'u kontrol et | Ball count = `StartBalls` (GameConfig varsayılanı veya upgrade değeri), coin = 0 | |
| 2.4 | Sahnedeki objeleri kontrol et | Yolda gate'ler ve obstacle'lar görünür, spawn exception yok | |
| 2.5 | Ekrana dokun / drag et | Player grubu ileri hareket eder, drag ile yatay kontrol çalışır | |

**Geçme kriteri:** Level spawn hatasız, HUD doğru count'u gösteriyor, hareket çalışıyor.

---

## ADIM 3 — Gate ve Count Doğrulaması (1:30–2:30)

| # | Aksiyon | Beklenen Çıktı | Hata mı? |
|---|---------|----------------|----------|
| 3.1 | +N gate'e gir | Ball count tam olarak +N artar, bir defaya mahsus tetiklenir | |
| 3.2 | Aynı +N gate'e tekrar gir (mümkünse test et) | İkinci tetikleme olmamalı (idempotent) | |
| 3.3 | -N gate'e gir | Ball count tam olarak -N azalır (0'ın altına düşmez) | |
| 3.4 | x2 gate'e gir | Ball count 2 katına çıkar (80 cap'i aşmaz) | |
| 3.5 | HUD ball counter'ı kontrol et | Her gate geçişinde anlık güncelleniyor | |

**Geçme kriteri:** Üç gate türü de doğru değişim uygular, tek tetikleme garantisi var, count HUD ile senkron.

---

## ADIM 4 — Obstacle Etkileşimi (2:30–3:00)

| # | Aksiyon | Beklenen Çıktı | Hata mı? |
|---|---------|----------------|----------|
| 4.1 | SpinBarObstacle'a çarp | Ball count belirl bir miktar azalır, HUD güncellenir | |
| 4.2 | DamageWallObstacle'a çarp | Ball count azalır, sıfıra düşerse Fail tetiklenir | |
| 4.3 | Count = 0 iken Fail panelini kontrol et | Fail paneli göründü, Retry / (Rewarded) butonları aktif | |

**Geçme kriteri:** Obstacle damage BallGroupController üzerinden işlenir, Fail state doğru tetiklenir.

---

## ADIM 5 — Win Akışı ve Skor Dönüşümü (3:00–4:00)

| # | Aksiyon | Beklenen Çıktı | Hata mı? |
|---|---------|----------------|----------|
| 5.1 | Finish Zone'a ulaş (level bitiş alanı) | `FinishZone` tetiklenir, kalan ball → coin hesabı yapılır | |
| 5.2 | Win panelini kontrol et | Kazanılan coin gösteriliyor, Next Level butonu aktif | |
| 5.3 | `CurrencyManager` coin değerini kontrol et | Önceki değer + kazanılan = yeni beklenen değer | |
| 5.4 | Next Level'a bas | Level index 1 artar, `GameplayBootstrapper` Level 2'yi yükler | |
| 5.5 | Level 2 spawn'ı kontrol et | Farklı gate/obstacle konfigürasyonu, exception yok | |

**Geçme kriteri:** `RunResultService` doğru coin üretir, SaveManager persist eder, level ilerlemesi çalışır.

---

## ADIM 6 — Upgrade ve Economy Persistance (4:00–4:30)

| # | Aksiyon | Beklenen Çıktı | Hata mı? |
|---|---------|----------------|----------|
| 6.1 | Play'i durdur, yeniden bas | MainMenu'ye dön veya direkt Gameplay başlat | |
| 6.2 | Coin değerini kontrol et | Önceki run'dan kazanılan coin kaybolmadı (SaveManager) | |
| 6.3 | `UpgradeManager` üzerinden Start Balls upgrade uygula (Inspector ile) | Bir sonraki run başlangıcında ball count değişti | |

**Geçme kriteri:** Coin ve upgrade state, Play durdur-başlat arasında persist ediyor.

---

## ADIM 7 — Stres Mini-Turu (4:30–5:00)

| # | Aksiyon | Beklenen Çıktı | Hata mı? |
|---|---------|----------------|----------|
| 7.1 | Hızla 3 ardışık run at (Win ya da Fail) | Her run sonunda panel görünür, restart çalışır, exception yok | |
| 7.2 | Console'ı tara | `NullReferenceException`, `MissingReferenceException`, `IndexOutOfRange` yok | |
| 7.3 | Unity Profiler Memory (opsiyonel) | Pool sayesinde spike yok, allocation düz seyrediyor | |

**Geçme kriteri:** 3 ardışık run temiz konsola sahip, bellek spike yok.

---

## Kritik Bug Listesi (Sıfır Tolerans)

Aşağı durumlarda smoke test **FAIL** sayılır ve build dondurulur:

| Bug | Neden Kritik |
|-----|-------------|
| Ball count HUD'dan farklı (`BallGroupController` desync) | Single source of truth ihlali |
| Aynı gate iki kez uygulama (double trigger) | Economy/count bozulması |
| Revive birden fazla tüketildi | Monetization hata verme riski |
| `NullReferenceException` on scene load | Crash riski cihazda |
| Level 1 spawn'da prefab bulunamadı (`MissingReferenceException`) | `LevelRuntimeBuilder` / registry bağlantısı kopuk |
| Finish Zone'dan sonra coin sıfır kalıyor | `RunResultService` → `CurrencyManager` bağlantısı yok |

---

## Beklenen Normal Davranışlar (Kırmızı Değil)

- İlk frame'de kısa bir yükleme beklemesi — normaldir (scene load)
- Pooling ilk aktifleşmede anlık 1 frame alloc — normaldir
- Ball count 80'de donuyor (cap) — tasarım gereği
- Gemini/Codex tarafından oluşturulan level preset'lerde boşluklar — içerik sorunu, kritik değil

---

## Smoke Test Sonucu Kayıt Şablonu

```
Tarih     : 2026-04-16
Tester    : [İsim/AI]
Build/SHA : [git hash ya da "editor-only"]
Sonuç     : PASS / FAIL
Fail ise  : [Adım #] — [Ne oldu]
Notlar    : 
```

---

*Bu protokol P4-T003 kapsamında Claude Sonnet 4.6 tarafından oluşturulmuştur.*
