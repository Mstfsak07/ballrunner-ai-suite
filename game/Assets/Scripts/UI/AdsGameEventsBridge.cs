using BallRunner.Ads;
using BallRunner.Core;
using UnityEngine;

namespace BallRunner.UI
{
    public sealed class AdsGameEventsBridge : MonoBehaviour
    {
        [SerializeField] private AdsManager adsManager;

        private void OnEnable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnRunFinished += HandleRunFinished;
            }
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnRunFinished -= HandleRunFinished;
            }
        }

        private void HandleRunFinished(bool isWin)
        {
            if (adsManager == null)
            {
                return;
            }

            if (isWin)
            {
                return;
            }

            adsManager.OnRunFail();
        }
    }
}
