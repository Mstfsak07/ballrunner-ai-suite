using BallRunner.Ads;
using BallRunner.Core;
using BallRunner.Economy;
using UnityEngine;

namespace BallRunner.UI
{
    public sealed class ResultAdsBinder : MonoBehaviour
    {
        [SerializeField] private AdsManager adsManager;
        [SerializeField] private RunResultService runResultService;
        [SerializeField] private UpgradeManager upgradeManager;
        [SerializeField] private HUDRuntimeBinder hudRuntimeBinder;

        private void Awake()
        {
            if (adsManager == null)
            {
                return;
            }

            adsManager.OnRewardedCoinGranted += OnRewardedCoin;
            adsManager.OnReviveGranted += OnRewardedRevive;
        }

        private void OnDestroy()
        {
            if (adsManager == null)
            {
                return;
            }

            adsManager.OnRewardedCoinGranted -= OnRewardedCoin;
            adsManager.OnReviveGranted -= OnRewardedRevive;
        }

        private void OnRewardedCoin()
        {
            if (upgradeManager != null && runResultService != null)
            {
                upgradeManager.AddRunReward(runResultService.LastCoin);
            }

            if (hudRuntimeBinder != null)
            {
                hudRuntimeBinder.ForceRefresh();
            }
        }

        private void OnRewardedRevive()
        {
            GameManager.Instance?.ResumeAfterRevive();
            if (hudRuntimeBinder != null)
            {
                hudRuntimeBinder.ForceRefresh();
            }
        }
    }
}
