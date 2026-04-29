using Between.Data;
using Between.Level;
using Between.Player;
using Between.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Between.StateMachines
{
    public class GameplayState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly GameContext _gameContext;
        private readonly ViewManager _viewManager;
        private readonly ViewPrefabsData _viewPrefabsData;
        private readonly GameConfigData _gameConfigData;
        private readonly GameAudioData _gameAudioData;

        private LevelRoot _level;
        private FirstPersonController _player;
        private GhostObject[] _ghostObjects;
        private ScenePostProcessings _sceneVolumes;

        private GameplayView _gameplayView;
        private Coroutine _fadeOutCoroutine;
        private Coroutine _vignetteCoroutine;

        private bool _isStandingStill = false;
        private float _timer;

        private bool _hasPokiGameplayStarted;

        private readonly int _alphaProperty = Shader.PropertyToID("Transparency_Intensity");

        public GameplayState(StateMachine stateMachine, GameConfigData gameConfigData, GameAudioData gameAudioData, ViewPrefabsData viewPrefabsData,
            ViewManager viewManager, GameContext gameContext)
        {
            _stateMachine = stateMachine;
            _gameConfigData = gameConfigData;
            _gameAudioData = gameAudioData;
            _viewManager = viewManager;
            _viewPrefabsData = viewPrefabsData;
            _gameContext = gameContext;
        }

        public async Awaitable Enter()
        {
            _sceneVolumes = Object.FindAnyObjectByType<ScenePostProcessings>();
            _gameplayView = _viewManager.CreateView(_viewPrefabsData.GameplayView);
            _gameplayView.Show();

            _level = _gameContext.CurrentLevelRoot;
            _player = _gameContext.Player;
            _ghostObjects = _level.GhostObjects;

            foreach (GhostObject ghostObject in _ghostObjects)
            {
                ghostObject.OnPlayerWalkedThrough += RespwanPlayer;

                if (ghostObject.TryGetComponent(out AudioSource ghostAudioSource))
                {
                    ghostAudioSource.Play();
                }
            }

            _level.LevelFinish.OnPlayerReachedFinish += OnFinishReached;
            _player.OnPlayerWalk += OnPlayerWalked;

            await _gameplayView.FadeIn();

            InitializePlayerControls();
        }


        private void InitializePlayerControls()
        {
            _level.StartCoroutine(StartWaveEffect(true, false));

            Cursor.lockState = CursorLockMode.Locked;
            _player.TogglePlayerFreeze(false);
        }

        public void Execute()
        {
            SetupGhostVision();
        }

        public void Exit()
        {
            _level.LevelFinish.OnPlayerReachedFinish -= OnFinishReached;
            _player.OnPlayerWalk -= OnPlayerWalked;

            _player.SetMoveAbility(false);
            _player.SetLookAbility(false);

            foreach (GhostObject ghostObject in _ghostObjects)
            {
                ghostObject.OnPlayerWalkedThrough -= RespwanPlayer;
            }

            _viewManager.DestroyView(_viewPrefabsData.GameplayView);
            _gameplayView = null;

            Object.Destroy(_level.gameObject);
        }

        private void OnPlayerWalked()
        {
            if (!_hasPokiGameplayStarted)
            {
                PokiUnitySDK.Instance.gameplayStart();
                _hasPokiGameplayStarted = true;
            }
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

                float fillProgress = 1f - Mathf.Clamp01(_timer / _gameConfigData.StandingStillTime);
                _gameplayView.SetEyeFillAmount(fillProgress);

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

                    StartVignetteCrossfade(0f, 1f);
                }
            }
            else if (_player.GetPlayerVelocity() > 1f)
            {
                _isStandingStill = false;
                _timer = 0f;

                _gameplayView.SetEyeFillAmount(1f);

                StartVignetteCrossfade(1f, 0f);

                if (_fadeOutCoroutine != null)
                    return;

                _fadeOutCoroutine = _level.StartCoroutine(StartWaveEffect(true, false));
            }
        }

        private void StartVignetteCrossfade(float targetOriginal, float targetStanding)
        {
            if (_vignetteCoroutine != null)
            {
                _level.StopCoroutine(_vignetteCoroutine);
                _vignetteCoroutine = null;
            }

            _vignetteCoroutine = _level.StartCoroutine(CrossfadeVignette(targetOriginal, targetStanding));
        }

        private IEnumerator CrossfadeVignette(float targetOriginal, float targetStanding)
        {
            float elapsed = 0f;
            float duration = _gameConfigData.StandingStillTime;

            float startOriginal = _sceneVolumes.OriginalVolume.weight;
            float startStanding = _sceneVolumes.StandingStillVolume.weight;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                _sceneVolumes.OriginalVolume.weight = Mathf.Lerp(startOriginal, targetOriginal, t);
                _sceneVolumes.StandingStillVolume.weight = Mathf.Lerp(startStanding, targetStanding, t);

                yield return null;
            }

            _sceneVolumes.OriginalVolume.weight = targetOriginal;
            _sceneVolumes.StandingStillVolume.weight = targetStanding;
            _vignetteCoroutine = null;
        }

        private IEnumerator StartWaveEffect(bool fromNearest, bool makeVisible)
        {
            float trasparencyAmount = 1f;

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
                        makeVisible ? 0f : trasparencyAmount,
                        makeVisible ? trasparencyAmount : 0f));
                }

                yield return new WaitForSeconds(_gameConfigData.WaveDelay);
            }

        }

        private IEnumerator FadeObject(GameObject obj, float startAlpha, float endAlpha)
        {
            if (!obj.TryGetComponent(out Renderer renderer)) yield break;

            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
            float elapsed = 0f;

            while (elapsed < _gameConfigData.GhostObjectsFadeDuration)
            {
                elapsed += Time.deltaTime;
                float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / _gameConfigData.GhostObjectsFadeDuration);

                renderer.GetPropertyBlock(propBlock);
                propBlock.SetFloat(_alphaProperty, currentAlpha);
                renderer.SetPropertyBlock(propBlock);

                yield return null;
            }
        }

        private void RespwanPlayer()
        {
            _player.TogglePlayerFreeze(true);

            PokiUnitySDK.Instance.gameplayStop();
            _hasPokiGameplayStarted = false;

            AudioManager.Instance.PlaySound(_gameAudioData.BoneCrack);

            CharacterController player = _gameContext.Player.GetComponent<CharacterController>();

            player.enabled = false;

            _player.transform.position = _level.PlayerSpawnPoint.position;
            _player.transform.rotation = _level.PlayerSpawnPoint.rotation;

            player.enabled = true;

            InitializePlayerControls();
            _gameplayView.FadeIn();
        }

        private void OnFinishReached()
        {
            int currentLevelIndex = PlayerPrefs.GetInt(Constants.CURRENT_LEVEL_KEY, 0);
            PlayerPrefs.SetInt(Constants.CURRENT_LEVEL_KEY, currentLevelIndex + 1);

            PokiUnitySDK.Instance.gameplayStop();
            _hasPokiGameplayStarted = false;
            _stateMachine.TransitionTo(_stateMachine.GamePreparationState);
        }
    }
}