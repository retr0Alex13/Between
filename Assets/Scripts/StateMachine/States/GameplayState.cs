using Between.Data;
using Between.Level;
using Between.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Between.StateMachines
{
    public class GameplayState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly GameContext _gameContext;
        private readonly GameConfigData _gameConfigData;

        private LevelRoot _level;
        private FirstPersonController _player;
        private GhostObject[] _ghostObjects;

        private Coroutine _fadeOutCoroutine;

        private bool _isStandingStill = false;
        private float _timer;

        private readonly int _alphaProperty = Shader.PropertyToID("_BaseColor");

        public GameplayState(StateMachine stateMachine, GameConfigData gameConfigData, GameContext gameContext)
        {
            _stateMachine = stateMachine;
            _gameConfigData = gameConfigData;
            _gameContext = gameContext;
        }

        public void Enter()
        {
            _level = _gameContext.CurrentLevelRoot;
            _player = _gameContext.Player;
            _ghostObjects = _level.GhostObjects;

            foreach (GhostObject ghostObject in _ghostObjects)
            {
                ghostObject.OnPlayerWalkedThrough += RespwanPlayer;
            }

            _level.LevelFinish.OnPlayerReachedFinish += OnFinishReached;

            Cursor.lockState = CursorLockMode.Locked;
            _player.SetMoveAbility(true);
            _player.SetLookAbility(true);
        }

        public void Execute()
        {
            SetupGhostVision();
        }

        public void Exit()
        {
            _level.LevelFinish.OnPlayerReachedFinish -= OnFinishReached;

            _player.SetMoveAbility(false);
            _player.SetLookAbility(false);

            foreach (GhostObject ghostObject in _ghostObjects)
            {
                ghostObject.OnPlayerWalkedThrough -= RespwanPlayer;
            }

            Object.Destroy(_level.gameObject);
        }

        private void SetupGhostVision()
        {
            if (_ghostObjects.Length <= 0)
                return;

            if (_player.GetPlayerVelocity() < 1f)
            {
                if (_isStandingStill)
                    return;

                _timer += Time.deltaTime;

                if (_timer >= _gameConfigData.StandingStillTime)
                {
                    if (_fadeOutCoroutine != null)
                    {
                        _level.StopCoroutine(_fadeOutCoroutine);
                        _fadeOutCoroutine = null;
                    }

                    _level.StartCoroutine(StartWaveEffect(true, true));
                    _isStandingStill = true;
                    _timer = 0f;
                }
            }
            else if (_player.GetPlayerVelocity() > 1f)
            {
                _isStandingStill = false;
                _timer = 0f;

                if (_fadeOutCoroutine != null)
                    return;

                _fadeOutCoroutine = _level.StartCoroutine(StartWaveEffect(true, false));
            }
        }

        private IEnumerator StartWaveEffect(bool fromNearest, bool makeVisible)
        {
            List<GhostObject> sortedGhosts = new List<GhostObject>(_ghostObjects);

            sortedGhosts.Sort((a, b) =>
            {
                float distA = Vector3.Distance(_player.transform.position, a.transform.position);
                float distB = Vector3.Distance(_player.transform.position, b.transform.position);
                return fromNearest ? distA.CompareTo(distB) : distB.CompareTo(distA);
            });

            foreach (GhostObject ghost in sortedGhosts)
            {
                Renderer[] childRenderers = ghost.GetComponentsInChildren<Renderer>();

                foreach (Renderer childRender in childRenderers)
                {
                    _level.StartCoroutine(FadeObject(childRender.gameObject,
                        makeVisible ? 0f : 0.1f,
                        makeVisible ? 0.1f : 0f));
                }

                yield return new WaitForSeconds(_gameConfigData.WaveDelay);
            }

        }

        private IEnumerator FadeObject(GameObject obj, float startAlpha, float endAlpha)
        {
            if (!obj.TryGetComponent(out Renderer renderer)) yield break;

            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
            float elapsed = 0f;

            while (elapsed < _gameConfigData.FadeDuration)
            {
                elapsed += Time.deltaTime;
                float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / _gameConfigData.FadeDuration);

                renderer.GetPropertyBlock(propBlock);

                Color color = renderer.sharedMaterial.color;
                color.a = currentAlpha;

                propBlock.SetColor(_alphaProperty, color);
                renderer.SetPropertyBlock(propBlock);
                
                yield return null;
            }
        }

        private void RespwanPlayer()
        {
            CharacterController player = _gameContext.Player.GetComponent<CharacterController>();

            player.enabled = false;

            _player.transform.position = _level.PlayerSpawnPoint.position;
            _player.transform.rotation = _level.PlayerSpawnPoint.rotation;

            player.enabled = true;
        }

        private void OnFinishReached()
        {
            int currentLevelIndex = PlayerPrefs.GetInt(Constants.CURRENT_LEVEL_KEY, 0);
            PlayerPrefs.SetInt(Constants.CURRENT_LEVEL_KEY, currentLevelIndex + 1);

            _stateMachine.TransitionTo(_stateMachine.GamePreparationState);
        }
    }
}