using BallRunner.Core;
using BallRunner.Ads;
using BallRunner.Economy;
using UnityEngine;
using UnityEngine.UI;

namespace BallRunner.UI
{
    public sealed class ResultPanel : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Text titleText;
        [SerializeField] private Text scoreText;
        [SerializeField] private Text coinText;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button reviveButton;
        [SerializeField] private Button rewardedCoinButton;
        [SerializeField] private AdsManager adsManager;

        [SerializeField] private RunResultService runResultService;
        private bool currentIsWin;
        private bool rewardedCoinClaimed;

        private void Awake()
        {
            if (root != null)
            {
                root.SetActive(false);
            }

            if (retryButton != null)
            {
                retryButton.onClick.AddListener(OnRetryClicked);
            }

            if (nextButton != null)
            {
                nextButton.onClick.AddListener(OnNextClicked);
            }

            if (reviveButton != null)
            {
                reviveButton.onClick.AddListener(OnReviveClicked);
            }

            if (rewardedCoinButton != null)
            {
                rewardedCoinButton.onClick.AddListener(OnRewardedCoinClicked);
            }
        }

        public void Show(bool isWin)
        {
            currentIsWin = isWin;
            rewardedCoinClaimed = false;
            if (root != null)
            {
                root.SetActive(true);
            }

            if (titleText != null)
            {
                titleText.text = isWin ? "LEVEL COMPLETE" : "LEVEL FAILED";
            }

            if (runResultService != null)
            {
                if (scoreText != null)
                {
                    scoreText.text = $"Score: {runResultService.LastScore}";
                }

                if (coinText != null)
                {
                    coinText.text = $"Coin: {runResultService.LastCoin}";
                }
            }

            if (nextButton != null)
            {
                nextButton.gameObject.SetActive(isWin);
            }

            if (retryButton != null)
            {
                retryButton.gameObject.SetActive(true);
            }

            if (reviveButton != null)
            {
                reviveButton.gameObject.SetActive(!isWin && adsManager != null && adsManager.CanUseReviveRewarded());
            }

            if (rewardedCoinButton != null)
            {
                rewardedCoinButton.gameObject.SetActive(isWin && adsManager != null);
                rewardedCoinButton.interactable = true;
            }
        }

        public void Hide()
        {
            if (root != null)
            {
                root.SetActive(false);
            }
        }

        private static void OnRetryClicked()
        {
            GameManager.Instance?.StartGame();
        }

        private static void OnNextClicked()
        {
            GameManager.Instance?.StartGame();
        }

        private void OnReviveClicked()
        {
            if (adsManager == null || currentIsWin)
            {
                return;
            }

            adsManager.ShowRewardedRevive();
        }

        private void OnRewardedCoinClicked()
        {
            if (adsManager == null || !currentIsWin || rewardedCoinClaimed)
            {
                return;
            }

            rewardedCoinClaimed = true;
            if (rewardedCoinButton != null)
            {
                rewardedCoinButton.interactable = false;
            }
            adsManager.ShowRewardedDoubleCoin();
        }
    }
}
