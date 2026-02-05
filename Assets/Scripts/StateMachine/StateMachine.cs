using Between.Player;
using System;
using Void.StateMachines;

namespace Between.StateMachines
{
    [Serializable]
    public class StateMachine
    {
        public IState CurrentState { get; private set; }

        private readonly GameplayState _gameplayState;
        public GameplayState GameplayState => _gameplayState;

        public event Action<IState> stateChanged;

        public StateMachine(PlayerMovement player)
        {
            _gameplayState = new GameplayState(player);
            // create an instance for each state and pass in PlayerController
        }

        // set the starting state
        public void Initialize(IState state)
        {
            CurrentState = state;
            state.Enter();

            // notify other objects that state has changed
            stateChanged?.Invoke(state);
        }

        // exit this state and enter another
        public void TransitionTo(IState nextState)
        {
            CurrentState.Exit();
            CurrentState = nextState;
            nextState.Enter();

            // notify other objects that state has changed
            stateChanged?.Invoke(nextState);
        }

        // allow the StateMachine to update this state
        public void Execute()
        {
            if (CurrentState != null)
            {
                CurrentState.Execute();
            }
        }
    }
}