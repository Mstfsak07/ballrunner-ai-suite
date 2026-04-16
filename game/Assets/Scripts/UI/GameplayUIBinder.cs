using BallRunner.Balls;
using BallRunner.Core;
using BallRunner.Player;
using UnityEngine;

namespace BallRunner.UI
{
    public sealed class GameplayUIBinder : MonoBehaviour
    {
        [SerializeField] private BallGroupController ballGroup;
        [SerializeField] private HUDController hud;
        [SerializeField] private ResultPanel resultPanel;
        [SerializeField] private PlayerMover playerMover;

        private void Update()
        {
            if (ballGroup != null && hud != null)
            {
                hud.SetBallCount(ballGroup.CurrentCount);
            }
        }

        private void OnEnable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnRunFinished += OnGameFinished;
                GameManager.Instance.OnRunResumed += ResumeAfterRevive;
            }
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnRunFinished -= OnGameFinished;
                GameManager.Instance.OnRunResumed -= ResumeAfterRevive;
            }
        }

        public void OnGameFinished(bool isWin)
        {
            if (playerMover != null)
            {
                playerMover.enabled = false;
            }

            if (resultPanel != null)
            {
                resultPanel.Show(isWin);
            }
        }

        public void ResumeAfterRevive()
        {
            if (resultPanel != null)
            {
                resultPanel.Hide();
            }

            if (playerMover != null)
            {
                playerMover.enabled = true;
            }
        }
    }
}
