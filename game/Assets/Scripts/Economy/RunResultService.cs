using BallRunner.Core;
using UnityEngine;

namespace BallRunner.Economy
{
    public sealed class RunResultService : MonoBehaviour
    {
        [SerializeField] private int scorePerBall = 10;
        [SerializeField] private UpgradeManager upgradeManager;

        public int LastScore { get; private set; }
        public int LastCoin { get; private set; }

        public void ApplyFinish(int remainingBalls)
        {
            var safeBalls = Mathf.Max(0, remainingBalls);
            var economy = RuntimeConfigInstaller.EconomyConfig;

            var baseCoin = economy != null ? economy.BaseLevelCompleteCoin : 20;
            var bonusPerBall = economy != null ? economy.BaseFinishScoreCoin : 1;

            LastScore = safeBalls * scorePerBall;
            LastCoin = baseCoin + safeBalls * bonusPerBall;
            if (upgradeManager != null)
            {
                upgradeManager.AddRunReward(LastCoin);
            }

            var win = safeBalls > 0;
            GameManager.Instance?.FinishLevel(win);
        }
    }
}
