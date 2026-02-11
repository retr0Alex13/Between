using Between.Data;
using Between.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Void.StateMachines;

namespace Between.StateMachines
{
    public class GameplayState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly GameContext _gameContext;

        private LevelRoot _level;
        private FirstPersonController _player;
        private GameObject[] _ghostObjects;

        // Move to level config data later
        private float _waveDelay = 0.1f;
        private float _fadeDuration = 1f;
        private float _standingStillTime = 2f;

        private Coroutine _fadeOutCoroutine;

        private bool _isStandingStill = false;
        private float _timer;

        public GameplayState(StateMachine stateMachine, GameContext gameContext)
        {
            _stateMachine = stateMachine;
            _gameContext = gameContext;
        }

        public void Enter()
        {
            _level = _gameContext.CurrentLevelRoot;
            _player = _gameContext.Player;
            _ghostObjects = _level.GhostObjects;

            _player.SetMoveAbility(true);
            _player.SetLookAbility(true);
        }

        public void Execute()
        {
            if (_player.GetPlayerVelocity() < 1f)
            {
                if (_isStandingStill)
                    return;

                _timer += Time.deltaTime;

                if (_timer >= _standingStillTime)
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

        public void Exit()
        {

        }

        private IEnumerator StartWaveEffect(bool fromNearest, bool makeVisible)
        {
            List<GameObject> sortedGhosts = new List<GameObject>(_ghostObjects);

            if (fromNearest)
            {
                sortedGhosts.Sort((a, b) =>
                    Vector3.Distance(_player.transform.position, a.transform.position)
                    .CompareTo(Vector3.Distance(_player.transform.position, b.transform.position))
                    );
            }
            else
            {
                sortedGhosts.Sort((a, b) =>
                    Vector3.Distance(_player.transform.position, b.transform.position)
                    .CompareTo(Vector3.Distance(_player.transform.position, a.transform.position))
                    );
            }

            foreach (GameObject ghost in sortedGhosts)
            {
                if (makeVisible)
                {
                    _level.StartCoroutine(FadeObject(ghost, 0f, 1f));
                }
                else
                {
                    _level.StartCoroutine(FadeObject(ghost, 1f, 0f));
                }
                yield return new WaitForSeconds(_waveDelay);
            }
        }

        private IEnumerator FadeObject(GameObject obj, float startAlpha, float endAlpha)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            Material mat = renderer.material;

            float elapsed = 0f;

            while (elapsed < _fadeDuration)
            {
                elapsed += Time.deltaTime;
                float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / _fadeDuration);

                Color color = mat.color;
                color.a = currentAlpha;
                mat.color = color;

                yield return null;
            }
        }
    }
}