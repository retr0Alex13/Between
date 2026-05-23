using Between.Data;
using Between.StateMachines;
using Between.View;
using UnityEngine;

namespace Between.StateMachines
{
    public class PauseState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly ViewManager _viewManager;
        private readonly ViewPrefabsData _viewPrefabsData;
        private readonly GameAudioData _gameAudioData;
        private PauseView _pauseView;

        private bool _isSoundEnabled;
        private bool _isMusicEnabled;

        public PauseState(StateMachine stateMachine, ViewPrefabsData viewPrefabsData, ViewManager viewManager, GameAudioData gameAudioData)
        {
            _stateMachine = stateMachine;
            _viewPrefabsData = viewPrefabsData;
            _viewManager = viewManager;
            _gameAudioData = gameAudioData;
        }

        public async Awaitable Enter()
        {
            _isMusicEnabled = PlayerPrefs.GetInt(Constants.MUSIC_SETTING_KEY, 1) == 1;
            _isSoundEnabled = PlayerPrefs.GetInt(Constants.SOUND_SETTING_KEY, 1) == 1;

            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            AudioListener.pause = true;

            _pauseView = _viewManager.CreateView(_viewPrefabsData.PauseView);
            _pauseView.SetMusicIcon(_isMusicEnabled);
            _pauseView.SetSoundIcon(_isSoundEnabled);
            _pauseView.Show();
            _pauseView.OnPlayButtonPressed += OnPlayButtonClicked;
            _pauseView.OnSoundButtonPressed += OnSoundButtonClicked;
            _pauseView.OnMusicButtonPressed += OnMusicButtonClicked;
        }

        public void Exit()
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            AudioListener.pause = false;
            AudioListener.volume = _isSoundEnabled ? 1f : 0f;

            _pauseView.OnPlayButtonPressed -= OnPlayButtonClicked;
            _pauseView.OnSoundButtonPressed -= OnSoundButtonClicked;
            _pauseView.OnMusicButtonPressed -= OnMusicButtonClicked;

            _viewManager.DestroyView(_viewPrefabsData.PauseView);
            _pauseView = null;
        }

        private void OnMusicButtonClicked()
        {
            _isMusicEnabled = !_isMusicEnabled;
            PlayerPrefs.SetInt(Constants.MUSIC_SETTING_KEY, _isMusicEnabled ? 1 : 0);

            if (_isMusicEnabled)
            {
                AudioManager.Instance.PlayMusic(_gameAudioData.SpookyGameplayMusic);
            }
            else
            {
                AudioManager.Instance.StopMusic();
            }

            _pauseView.SetMusicIcon(_isMusicEnabled);
        }

        private void OnSoundButtonClicked()
        {
            _isSoundEnabled = !_isSoundEnabled;
            _isMusicEnabled = _isSoundEnabled;

            PlayerPrefs.SetInt(Constants.SOUND_SETTING_KEY, _isSoundEnabled ? 1 : 0);
            PlayerPrefs.SetInt(Constants.MUSIC_SETTING_KEY, _isMusicEnabled ? 1 : 0);

            if (_isSoundEnabled)
            {
                AudioListener.volume = 1f;
                AudioManager.Instance.PlayMusic(_gameAudioData.SpookyGameplayMusic);
            }
            else
            {
                AudioListener.volume = 0f;
                AudioManager.Instance.StopMusic();
            }

            _pauseView.SetSoundIcon(_isSoundEnabled);
            _pauseView.SetMusicIcon(_isMusicEnabled);
        }

        private void OnPlayButtonClicked()
        {
            Resume();
        }

        private void Resume()
        {
            _stateMachine.PopOverlay();
        }

        private void Restart() => _stateMachine.TransitionTo(_stateMachine.GamePreparationState);
    }

}