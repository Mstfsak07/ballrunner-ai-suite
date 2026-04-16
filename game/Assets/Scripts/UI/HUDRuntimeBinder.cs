using BallRunner.Core;
using BallRunner.Economy;
using BallRunner.Level;
using UnityEngine;

namespace BallRunner.UI
{
    public sealed class HUDRuntimeBinder : MonoBehaviour
    {
        [SerializeField] private HUDController hud;
        [SerializeField] private CurrencyManager currencyManager;
        [SerializeField] private GameplayBootstrapper gameplayBootstrapper;
        [SerializeField] private Transform playerRoot;
        [SerializeField] private Transform finishTransform;

        private float runStartZ;
        private float runTargetDistance;

        private void Start()
        {
            if (playerRoot != null)
            {
                runStartZ = playerRoot.position.z;
            }

            if (finishTransform != null && playerRoot != null)
            {
                runTargetDistance = Mathf.Max(1f, finishTransform.position.z - runStartZ);
            }

            if (currencyManager != null)
            {
                currencyManager.OnCoinChanged += HandleCoinChanged;
                HandleCoinChanged(currencyManager.Coin);
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnRunResumed += ResetProgressOrigin;
            }

            if (gameplayBootstrapper != null)
            {
                gameplayBootstrapper.OnLevelChanged += HandleLevelChanged;
                HandleLevelChanged(gameplayBootstrapper.CurrentLevelIndex, gameplayBootstrapper.TotalLevelCount);
            }
        }

        private void OnDestroy()
        {
            if (currencyManager != null)
            {
                currencyManager.OnCoinChanged -= HandleCoinChanged;
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnRunResumed -= ResetProgressOrigin;
            }

            if (gameplayBootstrapper != null)
            {
                gameplayBootstrapper.OnLevelChanged -= HandleLevelChanged;
            }
        }

        private void Update()
        {
            UpdateProgress();
        }

        public void ForceRefresh()
        {
            if (currencyManager != null)
            {
                HandleCoinChanged(currencyManager.Coin);
            }

            UpdateProgress();
        }

        private void UpdateProgress()
        {
            if (hud == null || playerRoot == null || finishTransform == null)
            {
                return;
            }

            var traveled = Mathf.Max(0f, playerRoot.position.z - runStartZ);
            var normalized = runTargetDistance <= 0f ? 0f : traveled / runTargetDistance;
            hud.SetProgress(normalized);
        }

        private void HandleCoinChanged(int coin)
        {
            if (hud != null)
            {
                hud.SetCoin(coin);
            }
        }

        private void ResetProgressOrigin()
        {
            if (playerRoot != null)
            {
                runStartZ = playerRoot.position.z;
            }

            if (finishTransform != null && playerRoot != null)
            {
                runTargetDistance = Mathf.Max(1f, finishTransform.position.z - runStartZ);
            }
        }

        private void HandleLevelChanged(int levelIndex, int totalCount)
        {
            if (hud != null)
            {
                hud.SetLevel(levelIndex, totalCount);
            }
        }
    }
}
