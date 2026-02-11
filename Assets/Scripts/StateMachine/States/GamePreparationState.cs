using Between.Data;
using Between.Player;
using UnityEngine;
using Void.StateMachines;

namespace Between.StateMachines
{
    public class GamePreparationState : IState
    {
        private readonly GameObjectsData _gameObjectsData;
        private readonly LevelRoot[] _levels;
        private readonly FirstPersonController _player;
        private readonly GameContext _gameContext;
        private readonly StateMachine _stateMachine;

        private LevelRoot _currentLevel;
        private FirstPersonController _playerInstance;

        public GamePreparationState(StateMachine stateMachine, GameObjectsData gameObjectsData, GameContext gameContext)
        {
            _stateMachine = stateMachine;
            _gameObjectsData = gameObjectsData;
            _gameContext = gameContext;

            _levels = _gameObjectsData.Levels;
            _player = _gameObjectsData.PlayerPrefab;
        }

        public void Enter()
        {
            _currentLevel = Object.Instantiate(_levels[0]);
            _playerInstance = Object.Instantiate(_player, _currentLevel.PlayerSpawnPoint.position, Quaternion.identity);

            _playerInstance.SetLookAbility(false);
            _playerInstance.SetMoveAbility(false);

            _gameContext.Player = _playerInstance;
            _gameContext.CurrentLevelRoot = _currentLevel;

            _stateMachine.TransitionTo(_stateMachine.GameplayState);
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }
}
