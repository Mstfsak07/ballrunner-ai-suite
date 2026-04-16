using BallRunner.Core;
using UnityEngine;

namespace BallRunner.Economy
{
    public sealed class UpgradeManager : MonoBehaviour
    {
        [SerializeField] private CurrencyManager currencyManager;
        [SerializeField] private SaveManager saveManager;

        private PlayerProgressData progress;

        public int StartBallsLevel => progress.startBallsLevel;
        public int CoinBonusLevel => progress.coinBonusLevel;

        private void Awake()
        {
            progress = saveManager.Load();
            currencyManager.Initialize(progress);
        }

        public int GetStartBallsValue()
        {
            return 5 + progress.startBallsLevel;
        }

        public float GetCoinBonusMultiplier()
        {
            return 1f + progress.coinBonusLevel * 0.1f;
        }

        public bool TryUpgradeStartBalls()
        {
            var cost = GetStartBallsUpgradeCost(progress.startBallsLevel + 1);
            if (!currencyManager.TrySpend(cost))
            {
                return false;
            }

            progress.startBallsLevel++;
            Persist();
            return true;
        }

        public bool TryUpgradeCoinBonus()
        {
            var cost = GetCoinBonusUpgradeCost(progress.coinBonusLevel + 1);
            if (!currencyManager.TrySpend(cost))
            {
                return false;
            }

            progress.coinBonusLevel++;
            Persist();
            return true;
        }

        public void AddRunReward(int coin)
        {
            var rewarded = Mathf.RoundToInt(coin * GetCoinBonusMultiplier());
            currencyManager.Add(rewarded);
            Persist();
        }

        private static int GetStartBallsUpgradeCost(int targetLevel)
        {
            return targetLevel switch
            {
                1 => 50,
                2 => 100,
                3 => 200,
                4 => 350,
                _ => 500
            };
        }

        private static int GetCoinBonusUpgradeCost(int targetLevel)
        {
            return targetLevel switch
            {
                1 => 50,
                2 => 100,
                3 => 200,
                4 => 350,
                _ => 500
            };
        }

        private void Persist()
        {
            currencyManager.Persist(progress);
        }
    }
}
