using Between.Data;
using UnityEngine;
using Void.StateMachines;

namespace Between.StateMachines
{
    public class GameplayState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly GameContext _gameContext;

        public GameplayState(StateMachine stateMachine, GameContext gameContext)
        {
            _stateMachine = stateMachine;
            _gameContext = gameContext;
        }

        public void Enter()
        {
            Debug.Log(_gameContext.CurrentLevel + " " + _gameContext.Player);
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }
}
