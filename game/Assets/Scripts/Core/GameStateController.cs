using System;
using UnityEngine;

namespace BallRunner.Core
{
    public sealed class GameStateController : MonoBehaviour
    {
        [SerializeField] private GameState initialState = GameState.Boot;

        public GameState CurrentState { get; private set; }
        public event Action<GameState, GameState> OnStateChanged;

        private void Awake()
        {
            CurrentState = initialState;
        }

        public bool TrySetState(GameState next)
        {
            if (CurrentState == next)
            {
                return false;
            }

            var previous = CurrentState;
            CurrentState = next;
            OnStateChanged?.Invoke(previous, next);
            return true;
        }
    }
}
