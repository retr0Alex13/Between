using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Between.View
{
    public class GameplayView : BaseView
    {
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
    }
}
