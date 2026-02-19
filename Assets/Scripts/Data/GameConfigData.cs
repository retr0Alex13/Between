using UnityEngine;

namespace Between.Data
{
    [CreateAssetMenu(fileName = "GameConfigData", menuName = "Scriptable Objects/GameConfigData")]
    public class GameConfigData : ScriptableObject
    {
        [SerializeField]
        private float _waveDelay = 0.1f;

        [SerializeField]
        private float _fadeDuration = 0.5f;

        [SerializeField]
        private float _standingStillTime = 1f;

        public float WaveDelay => _waveDelay;
        public float FadeDuration => _fadeDuration;
        public float StandingStillTime => _standingStillTime;
    }
}
