using UnityEngine;

namespace BallRunner.Core
{
    public sealed class RuntimeConfigInstaller : MonoBehaviour
    {
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private EconomyConfig economyConfig;

        public static GameConfig GameConfig { get; private set; }
        public static EconomyConfig EconomyConfig { get; private set; }

        private void Awake()
        {
            if (gameConfig == null)
            {
                Debug.LogError("[RuntimeConfigInstaller] GameConfig is not assigned.");
            }

            if (economyConfig == null)
            {
                Debug.LogError("[RuntimeConfigInstaller] EconomyConfig is not assigned.");
            }

            GameConfig = gameConfig;
            EconomyConfig = economyConfig;
        }
    }
}
