using System;
using Backend.Invoker;
using Backend.Registration;
using UnityEngine;

namespace Game
{
    public static class GameStateController
    {
        private static GameState _currentState;
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


            OnStateChanged?.Invoke(state);
            SignalRegistration<ISignalInvoke>.Resolve()?.SendCommandToChangeState((int) state);
        }
    }

    public enum GameState : int
    {
        MainMenu = 0,
        Game = 1,
        FinalScreen = 2,
    }
}