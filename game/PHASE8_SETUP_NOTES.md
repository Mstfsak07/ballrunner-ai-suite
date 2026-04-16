# Phase 8 Setup Notes

## Scope
- P8-T001: HUD level text binding
- P8-T002: Debug reset helpers

## HUD Level Setup
1. HUD canvas'ta `levelText` alanini olustur.
2. `HUDController.levelText` referansini inspector'dan bagla.
3. `HUDRuntimeBinder.gameplayBootstrapper` referansini bagla.
4. Level gecisinde `Level X/Y` metninin yenilendigini dogrula.

Checks:
- [ ] H1: Ilk acilista Level 1/N gorunuyor.
- [ ] H2: Win sonrasi bir sonraki levelde X artiyor.
- [ ] H3: Son levelde wrap oldugunda Level 1/N'e donuyor.
- [ ] H4: Revive sonrasi level metni stabil kaliyor.

## Debug Reset Setup
1. Scene'e `DebugProgressTools` componenti ekle (debug object uzerinde).
2. Context menu ile su methodlari cagir:
   - `Reset Level Progress`
   - `Reset Tutorial State`
   - `Reset Economy Save`
   - `Reset All Gameplay Progress`

Checks:
- [ ] D1: Level reset sonrasi oyun Level 1/N ile acilir.
- [ ] D2: Tutorial reset sonrasi hand yeniden gorunur.
- [ ] D3: Economy reset sonrasi coin/upgrade sifirlanir.
- [ ] D4: All reset sonrasi HUD stale kalmaz (gerekirse ForceRefresh).

## Risks / Workarounds
- `levelText` atanmamissa: HUD level alani bos kalir.
  - Fix: inspector baglantisini zorunlu checklist'e koy.
- Debug reset productionda acik kalirsa:
  - Fix: debug object'i dev-only scene profile ile sinirla.
- Reset sonrasi HUD stale deger:
  - Fix: resetten sonra `HUDRuntimeBinder.ForceRefresh()` cagir.
