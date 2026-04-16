# Onboarding 10-Level Preset (Phase 2)

Use this file to create 10 `LevelData` assets in Unity.

| Level | StartBalls | Gates (sequence)         | Obstacles                 | TargetSec |
|------:|-----------:|--------------------------|---------------------------|----------:|
| 1     | 5          | +5, +8                   | DamageWall(2)             | 20        |
| 2     | 5          | +6, x2                   | SpinBar(2)                | 22        |
| 3     | 6          | +8, -4, +10              | DamageWall(3)             | 24        |
| 4     | 6          | +6, x2, -8               | SpinBar(2), DamageWall(2) | 25        |
| 5     | 7          | +10, -6, x2              | SpinBar(3)                | 26        |
| 6     | 7          | +8, +10, -10             | DamageWall(4)             | 28        |
| 7     | 8          | +10, x2, -12             | SpinBar(3), Gap           | 30        |
| 8     | 8          | +12, -8, x2              | DamageWall(4), SpinBar(3) | 32        |
| 9     | 9          | +8, +12, -15, x2         | Gap, SpinBar(4)           | 35        |
| 10    | 10         | +10, -10, x2, -12        | SpinBar(4), DamageWall(5) | 38        |

Notes:
- First 3 levels should over-reward player.
- Levels 4-7 teach risk/reward gates.
- Levels 8-10 prepare for retention difficulty.
