using System;
using UnityEngine;

namespace BallRunner.Ads
{
    public sealed class AdsManager : MonoBehaviour
    {
        [SerializeField] private bool useMockAds = true;
        [SerializeField] private int interstitialEveryNFail = 3;

        private int failCount;
        private bool reviveConsumedThisRun;

        public event Action OnRewardedCoinGranted;
        public event Action OnReviveGranted;

        public void OnRunStart()
        {
            reviveConsumedThisRun = false;
        }

        public void OnRunFail()
        {
            failCount++;
            if (failCount % Mathf.Max(1, interstitialEveryNFail) == 0)
            {
                ShowInterstitial();
            }
        }

        public void ShowInterstitial()
        {
            if (useMockAds)
            {
                Debug.Log("[Ads] Interstitial shown (mock)");
                return;
            }

            // TODO: Integrate SDK interstitial show call.
        }

        public void ShowRewardedDoubleCoin()
        {
            if (useMockAds)
            {
                Debug.Log("[Ads] Rewarded coin shown (mock)");
                OnRewardedCoinGranted?.Invoke();
                return;
            }

            // TODO: Integrate SDK rewarded callback.
        }

        public bool CanUseReviveRewarded()
        {
            return !reviveConsumedThisRun;
        }

        public void ShowRewardedRevive()
        {
            if (!CanUseReviveRewarded())
            {
                return;
            }

            if (useMockAds)
            {
                reviveConsumedThisRun = true;
                Debug.Log("[Ads] Rewarded revive shown (mock)");
                OnReviveGranted?.Invoke();
                return;
            }

            // TODO: Integrate SDK revive rewarded callback.
        }
    }
}
