using BallRunner.Ads;
using UnityEngine;

namespace BallRunner.Ads
{
    // Stub bridge for wiring a real SDK (AdMob/UnityAds/IronSource etc.).
    // Replace simulated callbacks with SDK show methods and completion handlers.
    public sealed class AdsSdkBridgeStub : MonoBehaviour
    {
        [SerializeField] private AdsManager adsManager;
        [SerializeField] private bool simulateSuccessInEditor = true;

        private void Awake()
        {
            if (adsManager == null)
            {
                adsManager = GetComponent<AdsManager>();
            }
        }

        private void OnEnable()
        {
            if (adsManager == null)
            {
                return;
            }

            adsManager.OnInterstitialRequested += HandleInterstitialRequested;
            adsManager.OnRewardedCoinRequested += HandleRewardedCoinRequested;
            adsManager.OnRewardedReviveRequested += HandleRewardedReviveRequested;
        }

        private void OnDisable()
        {
            if (adsManager == null)
            {
                return;
            }

            adsManager.OnInterstitialRequested -= HandleInterstitialRequested;
            adsManager.OnRewardedCoinRequested -= HandleRewardedCoinRequested;
            adsManager.OnRewardedReviveRequested -= HandleRewardedReviveRequested;
        }

        private void HandleInterstitialRequested()
        {
            // SDK hook point: call interstitial show API here.
            Debug.Log("[AdsSdkBridgeStub] Interstitial request received.");
        }

        private void HandleRewardedCoinRequested()
        {
            // SDK hook point: call rewarded coin show API and completion callback here.
            Debug.Log("[AdsSdkBridgeStub] Rewarded coin request received.");
            if (simulateSuccessInEditor)
            {
                adsManager.ResolveRewardedCoin(true);
            }
        }

        private void HandleRewardedReviveRequested()
        {
            // SDK hook point: call rewarded revive show API and completion callback here.
            Debug.Log("[AdsSdkBridgeStub] Rewarded revive request received.");
            if (simulateSuccessInEditor)
            {
                adsManager.ResolveRewardedRevive(true);
            }
        }
    }
}
