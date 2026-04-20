using Between.Data;
using Between.Level;
using Between.Player;
using UnityEngine;

namespace Between.StateMachines
{
    public class GamePreparationState : IState
    {
        private readonly GameObjectsData _gameObjectsData;
        private readonly GameAudioData _gameAudioData;
        private readonly LevelRoot[] _levels;
        private readonly FirstPersonController _player;
        private readonly GameContext _gameContext;
        private readonly StateMachine _stateMachine;

        private LevelRoot _currentLevel;
        private FirstPersonController _playerInstance;

        public GamePreparationState(StateMachine stateMachine, GameObjectsData gameObjectsData, GameAudioData gameAudioData,
            GameContext gameContext)
        {
            _stateMachine = stateMachine;
            _gameObjectsData = gameObjectsData;
            _gameAudioData = gameAudioData;
            _gameContext = gameContext;

            _levels = _gameObjectsData.Levels;
            _player = _gameObjectsData.PlayerPrefab;
        }

        public void Enter()
        {
            int currentLevelIndex = PlayerPrefs.GetInt(Constants.CURRENT_LEVEL_KEY, 0);

            if (currentLevelIndex > _levels.Length - 1)
            {
                currentLevelIndex = Random.Range(4, _levels.Length - 1);
                PlayerPrefs.SetInt(Constants.CURRENT_LEVEL_KEY, currentLevelIndex);
            }

            _currentLevel = Object.Instantiate(_levels[currentLevelIndex]);

            if (_playerInstance == null)
            {
                _playerInstance = Object.Instantiate(_player, _currentLevel.PlayerSpawnPoint.position, _currentLevel.PlayerSpawnPoint.rotation);

                _playerInstance.SetLookAbility(false);
                _playerInstance.SetMoveAbility(false);

                _gameContext.Player = _playerInstance;
            }
            else
            {
                _gameContext.Player.SetLookAbility(false);
                _gameContext.Player.SetMoveAbility(false);

                CharacterController player = _playerInstance.GetComponent<CharacterController>();
                player.enabled = false;

                _playerInstance.transform.position = _currentLevel.PlayerSpawnPoint.position;
                _playerInstance.transform.rotation = _currentLevel.PlayerSpawnPoint.rotation;

                player.enabled = true;
            }
            foreach (GhostObject ghostObject in _currentLevel.GhostObjects)
            {
                if (ghostObject.TryGetComponent(out AudioSource ghostAudioSource))
                {
                    ghostAudioSource.clip = _gameAudioData.GhostCry;
                }
            }
            if (_playerInstance.TryGetComponent(out FootstepPlayer playerFootsteps))
            {
                playerFootsteps.SetFootstepsAudio(_gameAudioData.FootstepAudioClips);
            }
            _gameContext.CurrentLevelRoot = _currentLevel;
            _stateMachine.TransitionTo(_stateMachine.GameplayState);
        }
    }
}
