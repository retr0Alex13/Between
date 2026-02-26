using Between.Data;
using Between.Level;
using Between.View;
using System;

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

        public StateMachine(ViewManager viewManager, ViewPrefabsData viewPrefabsData, GameObjectsData gameObjectsData, GameConfigData gameConfigData)
        {
            _gameContext = new GameContext();

            _gamePreparationState = new GamePreparationState(this, gameObjectsData, _gameContext);
            _gameplayState = new GameplayState(this, gameConfigData, viewPrefabsData, viewManager, _gameContext);
        }

        public void Initialize(IState state)
        {
            CurrentState = state;
            state.Enter();

            stateChanged?.Invoke(state);
        }

        public void TransitionTo(IState nextState)
        {
            CurrentState.Exit();
            CurrentState = nextState;
            nextState.Enter();

            stateChanged?.Invoke(nextState);
        }

        public void Execute()
        {
            if (CurrentState != null)
            {
                CurrentState.Execute();
            }
        }
    }
}