# Phase 9 Wiring Control Notes

## Scope
- P9-T001: SceneReferenceValidator
- P9-T002: DebugHotkeys

## 1) SceneReferenceValidator Setup
- Component: `SceneReferenceValidator`
- Purpose: Play baslangicinda kritik referans eksiklerini raporlamak

Required references:
- `gameplayBootstrapper`
- `levelCatalog`
- `levelRuntimeBuilder`
- `playerMover`
- `ballGroupController`
- `finishZone`
- `currencyManager`
- `upgradeManager`
- `adsManager`
- `hudController`
- `hudRuntimeBinder`
- `resultPanel`
- `resultAdsBinder`

Validation checks:
- [ ] V1: Tum referanslar doluyken "All critical references are assigned" logu gelir.
- [ ] V2: Bir referans bos birakildiginda warning listesinde ilgili alan gorunur.
- [ ] V3: Oyun eksik referans olsa da crash olmaz.
- [ ] V4: Eksikler tamamlandiktan sonra warning temizlenir.

## 2) DebugHotkeys Setup
- Component: `DebugHotkeys`
- Purpose: Play mode test hizlandirma

Hotkeys:
- `F5`: Replay current level
- `F6`: Reset all gameplay progress
- `F7`: Simulate fail
- `F8`: Simulate win
- `F9`: Trigger rewarded x2
- `F10`: Trigger rewarded revive

Validation checks:
- [ ] H1: F5 mevcut leveli yeniden yukler.
- [ ] H2: F6 level/tutorial/economy save keylerini sifirlar.
- [ ] H3: F7 fail panelini acar.
- [ ] H4: F8 win panelini acar.
- [ ] H5: F9 rewarded x2 akisini tetikler.
- [ ] H6: F10 revive akisini tetikler.
- [ ] H7: Hotkeys referans eksikliginde crash yerine güvenli davranir.

## Risks / Workarounds
- Risk: Debug tools productionda acik kalabilir.
  - Workaround: Scene profilinde sadece dev buildlerde aktif et.
- Risk: Missing reference spam log.
  - Workaround: Validator warninglerini once temizleyip sonra smoke test calistir.
- Risk: Hotkey ile state atlama test sonucunu kirletir.
  - Workaround: Her hotkey testinden sonra F6 + clean restart yap.
