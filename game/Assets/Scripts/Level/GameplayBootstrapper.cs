using BallRunner.Ads;
using BallRunner.Balls;
using BallRunner.Core;
using BallRunner.Data;
using BallRunner.Economy;
using System;
using UnityEngine;

namespace BallRunner.Level
{
    public sealed class GameplayBootstrapper : MonoBehaviour
    {
        private const string LevelIndexKey = "ball_runner_level_index";

        [SerializeField] private LevelCatalog levelCatalog;
        [SerializeField] private LevelRuntimeBuilder levelRuntimeBuilder;
        [SerializeField] private BallGroupController ballGroup;
        [SerializeField] private UpgradeManager upgradeManager;
        [SerializeField] private AdsManager adsManager;

        private int currentLevelIndex;
        public event Action<int, int> OnLevelChanged;

        private void Start()
        {
            currentLevelIndex = Mathf.Max(0, PlayerPrefs.GetInt(LevelIndexKey, 0));
            BuildCurrentLevel();

            if (adsManager != null)
            {
                adsManager.OnRunStart();
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnRunFinished += HandleRunFinished;
            }
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnRunFinished -= HandleRunFinished;
            }
        }

        private void BuildCurrentLevel()
        {
            if (levelCatalog == null)
            {
                Debug.LogError("[GameplayBootstrapper] LevelCatalog missing.");
                return;
            }

            if (levelCatalog.LevelCount <= 0)
            {
                Debug.LogWarning("[GameplayBootstrapper] No level registered in catalog.");
                return;
            }

            currentLevelIndex = Mathf.Max(0, currentLevelIndex % levelCatalog.LevelCount);
            var data = levelCatalog.GetLevel(currentLevelIndex);
            if (data == null)
            {
                Debug.LogWarning("[GameplayBootstrapper] LevelData missing for current index.");
                return;
            }

            if (levelRuntimeBuilder != null)
            {
                levelRuntimeBuilder.Build(data);
            }

            var startBalls = data.StartBallCount;
            if (upgradeManager != null)
            {
                startBalls = Mathf.Max(startBalls, upgradeManager.GetStartBallsValue());
            }

            if (ballGroup != null)
            {
                ballGroup.Initialize(startBalls);
            }

            OnLevelChanged?.Invoke(currentLevelIndex, levelCatalog.LevelCount);
            PersistLevelIndex();
        }

        private void HandleRunFinished(bool isWin)
        {
            if (!isWin)
            {
                return;
            }

            currentLevelIndex++;
            if (levelCatalog != null && levelCatalog.LevelCount > 0)
            {
                currentLevelIndex %= levelCatalog.LevelCount;
            }
            PersistLevelIndex();
        }

        public void ResetProgress()
        {
            currentLevelIndex = 0;
            BuildCurrentLevel();
        }

        public void ReplayCurrentLevel()
        {
            BuildCurrentLevel();
        }

        private void PersistLevelIndex()
        {
            PlayerPrefs.SetInt(LevelIndexKey, currentLevelIndex);
            PlayerPrefs.Save();
        }
    }
}
