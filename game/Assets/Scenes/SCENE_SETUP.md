# Phase 0 Scene Bootstrap

Unity icinde olusturulacak sahneler:
- `Boot.unity`
- `MainMenu.unity`
- `Gameplay.unity`

## Boot scene
1. Bos bir `Bootstrap` GameObject olustur.
2. `BootLoader` component ekle.
3. Build Settings'te sahne sirasi: Boot -> MainMenu -> Gameplay.

## MainMenu scene
1. `App` GameObject olustur.
2. `RuntimeConfigInstaller` component ekle.
3. `GameConfig` ve `EconomyConfig` assetlerini inspector'dan bagla.
4. `GameStateController` ve `GameManager` ekle.
5. Play butonunu `GameManager.StartGame()` ile bagla.

## Gameplay scene
1. `GameplayRoot` olustur.
2. Test icin fail/win buttonlarini `GameManager.FinishLevel(bool)` ile bagla.

## Notes
- Bu fazda sadece state gecisi ve scene akisi hedeflenir.
- Ball, gate, obstacle sistemleri Phase 1'de eklenecek.

## Gameplay temporary movement test
1. `PlayerRoot` GameObject olustur.
2. `PlayerMover` component ekle.
3. Main Camera'ya `FollowTargetCamera` ekle ve target olarak `PlayerRoot` bagla.
4. Play mode'da otomatik ileri akis + yatay drag kontrolunu dogrula.

## Ball group setup
1. `PlayerRoot` altina `BallGroup` object ekle.
2. `BallGroupController` component ekle.
3. `BallUnit` prefab'i bagla ve initial count test et.
4. Runtime'da `ApplyDelta`/`ApplyMultiplier` cagriyla count degisimini dogrula.

## Gate setup
1. Trigger collider'li gate prefableri olustur.
2. `AddGate`, `SubtractGate`, `MultiplyGate` scriptlerini ilgili prefab'e bagla.
3. Player gate'i bir kez gectiginde tek uygulama oldugunu (idempotent trigger) dogrula.

## Obstacle setup
1. Donen cubuk prefab'ine `SpinBarObstacle` ekle.
2. Hasar duvari prefab'ine `DamageWallObstacle` ekle.
3. Temas basina azaltim miktarini `damage` ile ayarla.

## Finish setup
1. `RunResultService` componentini `GameplayRoot` uzerine ekle.
2. Finiş collider'ina `FinishZone` ekle.
3. `FinishZone.runResultService` referansini bagla.
4. Finişte `GameState`in `Win` veya `Fail`e gectigini dogrula.

## HUD and result UI setup
1. Canvas altina HUD text/slider alanlarini olustur ve `HUDController`a bagla.
2. Win/Fail panelini `ResultPanel` scripti ile bagla.
3. `ResultPanel` icin `reviveButton` ve `rewardedCoinButton` referanslarini bagla.
4. `GameplayUIBinder` ekleyip `BallGroupController`, `HUDController`, `ResultPanel`, `PlayerMover` referanslarini doldur.
5. Failde `revive` ile panel kapanip run kaldigi yerden devam ettigini dogrula.

## Economy and upgrade setup
1. `SaveManager`, `CurrencyManager`, `UpgradeManager` componentlerini `App` veya `GameplayRoot` uzerine ekle.
2. `UpgradeManager` icindeki manager referanslarini inspector'da bagla.
3. `RunResultService.upgradeManager` alanini baglayarak run coin reward akisini aktif et.

## Pooling setup
1. `BallPool` componentini ayri bir object'e ekle ve `BallUnit` prefab'ini bagla.
2. `BallGroupController.pool` referansini bu pool'a bagla.
3. Runtime'da top azalip artarken yeni instantiate yerine pool kullanimini dogrula.

## Ads hooks setup
1. `AdsManager` componentini scene'e ekle (`useMockAds=true` ile test et).
2. `ResultAdsBinder` ile rewarded x2 coin ve revive eventlerini bagla.
3. `AdsGameEventsBridge` ile fail durumunda interstitial cadence tetikle.

## Runtime level build setup
1. `LevelCatalog` object ekle ve `LevelData` asset listesi bagla.
2. `LevelRuntimeBuilder` ekle, `LevelCatalog` referansini ver.
3. Gate/Obstacle prefab registry alanlarini enum tiplerine gore doldur.
4. Play mode'da level kaydindan gate/obstacle spawn oldugunu dogrula.

## Gameplay bootstrap setup
1. `GameplayBootstrapper` ekleyip `LevelCatalog`, `LevelRuntimeBuilder`, `BallGroupController`, `UpgradeManager`, `AdsManager` referanslarini bagla.
2. `LevelRuntimeBuilder.buildOnStart` degerini `false` yap (spawn iki kez olmasin).
3. Win sonrasi level index artisini `PlayerPrefs` uzerinden dogrula.
4. Opsiyonel debug butonlari icin `ResetProgress()` ve `ReplayCurrentLevel()` methodlarini baglayabilirsin.

## Live HUD binding
1. `HUDRuntimeBinder` ekle ve `HUDController`, `CurrencyManager`, `PlayerRoot`, `FinishTransform` bagla.
2. `HUDController` icindeki `levelText` referansini bagla.
3. Coin degisiminde HUD text'in anlik guncellendigini dogrula.
4. Progress slider'in player->finish mesafesine gore arttigini dogrula.
5. Level gecisinde `Level X/Y` metninin guncellendigini dogrula.
6. `ResultAdsBinder.hudRuntimeBinder` alanini baglayip rewarded/revive sonrasi `ForceRefresh`i dogrula.

## Tutorial hand
1. Canvas altinda hand gostergesi object'i olustur.
2. `TutorialHandController` ekleyip `handRoot` ve `PlayerMover` referanslarini bagla.
3. Ilk dragde hand gizlenmeli ve sonraki runlarda tekrar acilmamali.

## Debug reset tools
1. Scene'e `DebugProgressTools` ekle (opsiyonel).
2. Context menu'den level/tutorial/economy reset methodlarini test et.

## Scene reference validation
1. Scene'e `SceneReferenceValidator` ekle.
2. Kritik sistem referanslarini inspector'da bagla.
3. Play baslangicinda warning varsa eksik referanslari tamamla.

## Debug hotkeys (play mode)
1. Scene'e `DebugHotkeys` ekle.
2. `GameplayBootstrapper`, `DebugProgressTools`, `AdsManager` referanslarini bagla.
3. Kisa yollar:
   - `F5`: Replay current level
   - `F6`: Reset all progress
   - `F7`: Simulate fail
   - `F8`: Simulate win
   - `F9`: Trigger rewarded x2
   - `F10`: Trigger rewarded revive

## Build metadata
1. `BuildMetadata` ScriptableObject olustur.
2. `BuildMetadataLogger` componentini boot veya menu root'a ekleyip metadata referansini bagla.
3. Internal test oncesi metadata assetinde timestamp stamp et.
