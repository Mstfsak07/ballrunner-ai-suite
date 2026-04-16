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
        private bool awaitingRewardedCoin;
        private bool awaitingRewardedRevive;

        public event Action OnRewardedCoinGranted;
        public event Action OnReviveGranted;
        public event Action OnInterstitialRequested;
        public event Action OnRewardedCoinRequested;
        public event Action OnRewardedReviveRequested;

        public void OnRunStart()
        {
            reviveConsumedThisRun = false;
            awaitingRewardedCoin = false;
            awaitingRewardedRevive = false;
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

            if (OnInterstitialRequested == null)
            {
                Debug.LogWarning("[Ads] Interstitial requested but no SDK bridge is subscribed.");
                return;
            }

            OnInterstitialRequested.Invoke();
        }

        public void ShowRewardedDoubleCoin()
        {
            if (useMockAds)
            {
                Debug.Log("[Ads] Rewarded coin shown (mock)");
                OnRewardedCoinGranted?.Invoke();
                return;
            }

            if (awaitingRewardedCoin)
            {
                return;
            }

            if (OnRewardedCoinRequested == null)
            {
                Debug.LogWarning("[Ads] Rewarded coin requested but no SDK bridge is subscribed.");
                return;
            }

            awaitingRewardedCoin = true;
            OnRewardedCoinRequested.Invoke();
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

            if (awaitingRewardedRevive)
            {
                return;
            }

            if (OnRewardedReviveRequested == null)
            {
                Debug.LogWarning("[Ads] Rewarded revive requested but no SDK bridge is subscribed.");
                return;
            }

            awaitingRewardedRevive = true;
            OnRewardedReviveRequested.Invoke();
        }

        public void ResolveRewardedCoin(bool granted)
        {
            if (!awaitingRewardedCoin)
            {
                return;
            }

            awaitingRewardedCoin = false;
            if (granted)
            {
                OnRewardedCoinGranted?.Invoke();
            }
        }

        public void ResolveRewardedRevive(bool granted)
        {
            if (!awaitingRewardedRevive)
            {
                return;
            }

            awaitingRewardedRevive = false;
            if (!granted || reviveConsumedThisRun)
            {
                return;
            }

            reviveConsumedThisRun = true;
            OnReviveGranted?.Invoke();
        }
    }
}
