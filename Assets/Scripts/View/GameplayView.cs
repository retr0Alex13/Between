using UnityEngine;
using UnityEngine.UI;

namespace Between.View
{
    public class GameplayView : BaseView
    {
        [SerializeField]
        private Image _closedEye;

        public void SetEyeFillAmount(float amount)
        {
            _closedEye.fillAmount = amount;
        }
    }
}
