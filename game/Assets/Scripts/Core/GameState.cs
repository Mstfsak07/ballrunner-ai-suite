using System;

namespace BallRunner.Core
{
    public enum GameState
    {
        Boot = 0,
        MainMenu = 1,
        Playing = 2,
        Win = 3,
        Fail = 4,
        Revive = 5,
        Paused = 6
    }
}
