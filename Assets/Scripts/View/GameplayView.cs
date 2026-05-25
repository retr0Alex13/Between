using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Between.View
{
    public class GameplayView : BaseView
    {
        public event Action OnPauseButton;

        [SerializeField]
        private Button _pauseButton;

        [SerializeField]
        private Image _closedEye;

        [SerializeField]
        private float _blackScreenFadeSpeed = 0.2f;

        [SerializeField]
        private Image _blackScreen;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = _blackScreen.GetComponent<CanvasGroup>();
            _pauseButton.onClick.AddListener(OnPauseButtonClicked);
        }

        private void OnDestroy()
        {
            _pauseButton.onClick.RemoveAllListeners();
        }

        public void SetEyeFillAmount(float amount)
        {
            _closedEye.fillAmount = amount;
        }

        public async Awaitable FadeIn()
        {
            _canvasGroup.alpha = 1f;
            while (_canvasGroup.alpha != 0)
            {
                _canvasGroup.alpha -= _blackScreenFadeSpeed * Time.deltaTime;
                await Awaitable.NextFrameAsync();
            }
        }

        public void OnPauseButtonClicked()
        {
            OnPauseButton?.Invoke();
        }

        public void SetPauseButtonInteractable(bool isInteractable)
        {
            _pauseButton.interactable = isInteractable;
        }
    }
}
