using System;
using UnityEngine;
using UnityEngine.UI;

namespace Between.View
{
    public class PauseView : BaseView
    {
        [SerializeField]
        private Button _playButton;

        [SerializeField]
        private Button _soundButton;

        [SerializeField]
        private Button _musicButton;

        [SerializeField]
        private Sprite _musicIcon;

        [SerializeField]
        private Sprite _mutedMusicIcon;

        [SerializeField]
        private Sprite _soundIcon;

        [SerializeField]
        private Sprite _mutedSoundIcon;

        [SerializeField]
        private Image _musicSpriteRenderer;

        [SerializeField]
        private Image _soundSpriteRenderer;

        public event Action OnPlayButtonPressed;
        public event Action OnSoundButtonPressed;
        public event Action OnMusicButtonPressed;

        private void Awake()
        {
            _playButton.onClick.AddListener(OnPlayButtonClickedHandler);
            _soundButton.onClick.AddListener(OnSoundButtonClickedHandler);
            _musicButton.onClick.AddListener(OnMusicButtonClickedHandler);
        }

        void OnDestroy()
        {
            _playButton.onClick?.RemoveAllListeners();
            _soundButton.onClick?.RemoveAllListeners();
            _musicButton.onClick?.RemoveAllListeners();
        }

        private void OnPlayButtonClickedHandler()
        {
            OnPlayButtonPressed.Invoke();
        }

        private void OnSoundButtonClickedHandler()
        {
            OnSoundButtonPressed.Invoke();
        }

        private void OnMusicButtonClickedHandler()
        {
            OnMusicButtonPressed.Invoke();
        }

        public void SetMusicIcon(bool isMusicToggled)
        {
            if (isMusicToggled)
            {
                _musicSpriteRenderer.sprite = _musicIcon;
            }
            else
            {
                _musicSpriteRenderer.sprite = _mutedMusicIcon;
            }
        }

        public void SetSoundIcon(bool isSoundToggled)
        {
            if (isSoundToggled)
            {
                _soundSpriteRenderer.sprite = _soundIcon;
            }
            else
            {
                _soundSpriteRenderer.sprite = _mutedSoundIcon;
            }
        }
    }
}