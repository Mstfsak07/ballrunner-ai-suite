using UnityEngine;

namespace BallRunner.Core
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "BallRunner/Config/Game Config")]
    public sealed class GameConfig : ScriptableObject
    {
        [Header("General")]
        [SerializeField] private int targetFps = 60;
        [SerializeField] private bool verboseLogging;

        [Header("Gameplay")]
        [SerializeField] private int maxVisibleBalls = 80;
        [SerializeField] private float levelTargetDurationMin = 20f;
        [SerializeField] private float levelTargetDurationMax = 40f;

        public int TargetFps => targetFps;
        public bool VerboseLogging => verboseLogging;
        public int MaxVisibleBalls => maxVisibleBalls;
        public float LevelTargetDurationMin => levelTargetDurationMin;
        public float LevelTargetDurationMax => levelTargetDurationMax;
    }
}
