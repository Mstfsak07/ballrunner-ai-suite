using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace BallRunner.Core
{
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] private GameConfig config;
        [SerializeField] private GameStateController stateController;

        public static GameManager Instance { get; private set; }
        public event Action<bool> OnRunFinished;
        public event Action OnRunResumed;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (config == null)
            {
                config = RuntimeConfigInstaller.GameConfig;
            }

            if (config != null)
            {
                Application.targetFrameRate = config.TargetFps;
            }

            if (stateController == null)
            {
                stateController = GetComponent<GameStateController>();
            }

            if (stateController != null)
            {
                stateController.OnStateChanged += HandleStateChanged;
            }
        }

        private void OnDestroy()
        {
            if (stateController != null)
            {
                stateController.OnStateChanged -= HandleStateChanged;
            }
        }

        public void StartGame()
        {
            if (stateController != null)
            {
                stateController.TrySetState(GameState.Playing);
            }

            SceneManager.LoadScene(SceneIds.Gameplay);
        }

        public void ReturnToMenu()
        {
            if (stateController != null)
            {
                stateController.TrySetState(GameState.MainMenu);
            }

            SceneManager.LoadScene(SceneIds.MainMenu);
        }

        public void FinishLevel(bool isWin)
        {
            if (stateController == null)
            {
                return;
            }

            stateController.TrySetState(isWin ? GameState.Win : GameState.Fail);
            OnRunFinished?.Invoke(isWin);
        }

        public void ResumeAfterRevive()
        {
            if (stateController != null)
            {
                stateController.TrySetState(GameState.Revive);
                stateController.TrySetState(GameState.Playing);
            }

            OnRunResumed?.Invoke();
        }

        private void HandleStateChanged(GameState previous, GameState next)
        {
            if (config != null && config.VerboseLogging)
            {
                Debug.Log($"[GameManager] State changed {previous} -> {next}");
            }
        }
    }
}
