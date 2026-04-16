using BallRunner.Player;
using UnityEngine;

namespace BallRunner.UI
{
    public sealed class TutorialHandController : MonoBehaviour
    {
        private const string TutorialSeenKey = "ball_runner_tutorial_seen";

        [SerializeField] private GameObject handRoot;
        [SerializeField] private PlayerMover playerMover;

        private void Start()
        {
            var seen = PlayerPrefs.GetInt(TutorialSeenKey, 0) == 1;
            if (seen)
            {
                SetHandVisible(false);
                return;
            }

            SetHandVisible(true);
            if (playerMover != null)
            {
                playerMover.OnFirstInputDetected += HandleFirstInput;
            }
        }

        private void OnDestroy()
        {
            if (playerMover != null)
            {
                playerMover.OnFirstInputDetected -= HandleFirstInput;
            }
        }

        private void HandleFirstInput()
        {
            SetHandVisible(false);
            PlayerPrefs.SetInt(TutorialSeenKey, 1);
            PlayerPrefs.Save();
        }

        private void SetHandVisible(bool visible)
        {
            if (handRoot != null)
            {
                handRoot.SetActive(visible);
            }
        }
    }
}
