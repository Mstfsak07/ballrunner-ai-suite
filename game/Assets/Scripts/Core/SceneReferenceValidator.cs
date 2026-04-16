using System.Collections.Generic;
using BallRunner.Ads;
using BallRunner.Balls;
using BallRunner.Economy;
using BallRunner.Level;
using BallRunner.Player;
using BallRunner.UI;
using UnityEngine;

namespace BallRunner.Core
{
    public sealed class SceneReferenceValidator : MonoBehaviour
    {
        [Header("Core")]
        [SerializeField] private GameplayBootstrapper gameplayBootstrapper;
        [SerializeField] private LevelCatalog levelCatalog;
        [SerializeField] private LevelRuntimeBuilder levelRuntimeBuilder;

        [Header("Gameplay")]
        [SerializeField] private PlayerMover playerMover;
        [SerializeField] private BallGroupController ballGroupController;
        [SerializeField] private FinishZone finishZone;

        [Header("Economy/Ads")]
        [SerializeField] private CurrencyManager currencyManager;
        [SerializeField] private UpgradeManager upgradeManager;
        [SerializeField] private AdsManager adsManager;

        [Header("UI")]
        [SerializeField] private HUDController hudController;
        [SerializeField] private HUDRuntimeBinder hudRuntimeBinder;
        [SerializeField] private ResultPanel resultPanel;
        [SerializeField] private ResultAdsBinder resultAdsBinder;

        [Header("Behavior")]
        [SerializeField] private bool validateOnStart = true;

        private void Start()
        {
            if (validateOnStart)
            {
                ValidateReferences();
            }
        }

        [ContextMenu("Validate Scene References")]
        public void ValidateReferences()
        {
            var missing = new List<string>();

            Check(gameplayBootstrapper, nameof(gameplayBootstrapper), missing);
            Check(levelCatalog, nameof(levelCatalog), missing);
            Check(levelRuntimeBuilder, nameof(levelRuntimeBuilder), missing);
            Check(playerMover, nameof(playerMover), missing);
            Check(ballGroupController, nameof(ballGroupController), missing);
            Check(finishZone, nameof(finishZone), missing);
            Check(currencyManager, nameof(currencyManager), missing);
            Check(upgradeManager, nameof(upgradeManager), missing);
            Check(adsManager, nameof(adsManager), missing);
            Check(hudController, nameof(hudController), missing);
            Check(hudRuntimeBinder, nameof(hudRuntimeBinder), missing);
            Check(resultPanel, nameof(resultPanel), missing);
            Check(resultAdsBinder, nameof(resultAdsBinder), missing);

            if (missing.Count == 0)
            {
                Debug.Log("[SceneReferenceValidator] All critical references are assigned.");
                return;
            }

            Debug.LogWarning(
                "[SceneReferenceValidator] Missing references:\n - " + string.Join("\n - ", missing)
            );
        }

        private static void Check(Object reference, string label, ICollection<string> missing)
        {
            if (reference == null)
            {
                missing.Add(label);
            }
        }
    }
}
