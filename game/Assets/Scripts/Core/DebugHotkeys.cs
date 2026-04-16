using BallRunner.Ads;
using BallRunner.Level;
using UnityEngine;

namespace BallRunner.Core
{
    public sealed class DebugHotkeys : MonoBehaviour
    {
        [SerializeField] private bool enabledInPlayMode = true;
        [SerializeField] private GameplayBootstrapper gameplayBootstrapper;
        [SerializeField] private DebugProgressTools debugProgressTools;
        [SerializeField] private AdsManager adsManager;

        private void Update()
        {
            if (!enabledInPlayMode)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.F5))
            {
                gameplayBootstrapper?.ReplayCurrentLevel();
                Debug.Log("[DebugHotkeys] F5 ReplayCurrentLevel");
            }

            if (Input.GetKeyDown(KeyCode.F6))
            {
                debugProgressTools?.ResetAllGameplayProgress();
                gameplayBootstrapper?.ResetProgress();
                Debug.Log("[DebugHotkeys] F6 ResetAllGameplayProgress");
            }

            if (Input.GetKeyDown(KeyCode.F7))
            {
                GameManager.Instance?.FinishLevel(false);
                Debug.Log("[DebugHotkeys] F7 Simulate Fail");
            }

            if (Input.GetKeyDown(KeyCode.F8))
            {
                GameManager.Instance?.FinishLevel(true);
                Debug.Log("[DebugHotkeys] F8 Simulate Win");
            }

            if (Input.GetKeyDown(KeyCode.F9))
            {
                adsManager?.ShowRewardedDoubleCoin();
                Debug.Log("[DebugHotkeys] F9 Rewarded x2");
            }

            if (Input.GetKeyDown(KeyCode.F10))
            {
                adsManager?.ShowRewardedRevive();
                Debug.Log("[DebugHotkeys] F10 Rewarded Revive");
            }
        }
    }
}
