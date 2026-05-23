using Between.Data;
using Between.Level;
using Between.View;
using System;
using System.Collections.Generic;

namespace Between.StateMachines
{
    [Serializable]
    public class StateMachine
    {
        public GamePreparationState GamePreparationState => _gamePreparationState;
        public GameplayState GameplayState => _gameplayState;
        public PauseState PauseState => _pauseState;

        public IState CurrentState { get; private set; }

        private readonly GamePreparationState _gamePreparationState;
        private readonly GameplayState _gameplayState;
        private readonly PauseState _pauseState;

        private readonly Stack<IState> _overlayStack = new Stack<IState>();
        private readonly GameContext _gameContext;

        public event Action<IState> stateChanged;

        public StateMachine(ViewManager viewManager, ViewPrefabsData viewPrefabsData, GameObjectsData gameObjectsData,
            GameConfigData gameConfigData, GameAudioData gameAudioData)
        {
            _gameContext = new GameContext();

            _gamePreparationState = new GamePreparationState(this, gameObjectsData, gameAudioData, _gameContext);
            _gameplayState = new GameplayState(this, gameConfigData, gameAudioData, viewPrefabsData, viewManager, _gameContext);
            _pauseState = new PauseState(this, viewPrefabsData, viewManager, gameAudioData);
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

        public void PushOverlay(IState overlay)
        {
            _overlayStack.Push(CurrentState); 
            CurrentState = overlay;
            overlay.Enter();
            stateChanged?.Invoke(overlay);
        }

        public void PopOverlay()
        {
            if (_overlayStack.Count == 0) return;

            CurrentState.Exit();               
            CurrentState = _overlayStack.Pop();
            stateChanged?.Invoke(CurrentState);
        }
    }
}