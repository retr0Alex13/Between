using Between.Data;
using Between.Player;
using System;
using Void.StateMachines;

namespace Between.StateMachines
{
    [Serializable]
    public class StateMachine
    {
        public GamePreparationState GamePreparationState => _gamePreparationState;
        public GameplayState GameplayState => _gameplayState;

        public IState CurrentState { get; private set; }

        private readonly GamePreparationState _gamePreparationState;
        private readonly GameplayState _gameplayState;

        private readonly GameContext _gameContext;

        public event Action<IState> stateChanged;

        public StateMachine(GameObjectsData gameObjectsData)
        {
            _gameContext = new GameContext();

            _gamePreparationState = new GamePreparationState(this, gameObjectsData, _gameContext);
            _gameplayState = new GameplayState(this, _gameContext);
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