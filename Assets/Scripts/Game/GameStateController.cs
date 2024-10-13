using System;
using Backend.Invoker;
using Backend.Registration;

namespace Game
{
    public static class GameStateController
    {
        private static GameState _currentState = GameState.None;
        public static event Action<GameState> OnStateChanged;

        public static GameState CurrentState
        {
            get => _currentState;
            set => UpdateState(value);
        }

        private static void UpdateState(GameState state)
        {
            if (_currentState == state)
                return;

            _currentState = state;
            OnStateChanged?.Invoke(state);
            SignalRegistration<ISignalInvoke>.Resolve()?.SendCommandToChangeState((int) state);
        }
    }

    public enum GameState : int
    {
        None = -1,
        MainMenu = 0,
        Game = 1,
        FinalScreen = 2,
        Tutorial = 3,
    }
}