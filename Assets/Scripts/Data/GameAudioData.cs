using UnityEngine;

namespace Between.Data
{
    [CreateAssetMenu(fileName = "GameAudioData", menuName = "Scriptable Objects/GameAudioData")]
    public class GameAudioData : ScriptableObject
    {
        [Header("SFX")]
        [SerializeField]
        private AudioClip[] _footstepAudioClips;

        [SerializeField]
        private AudioClip _boneCrack;

        [SerializeField]
        private AudioClip _ghostCry;

        [Header("Music")]
        [SerializeField]
        private AudioClip _spookyGameplayMusic;

        public AudioClip[] FootstepAudioClips => _footstepAudioClips;
        public AudioClip BoneCrack => _boneCrack;
        public AudioClip GhostCry => _ghostCry;
        public AudioClip SpookyGameplayMusic => _spookyGameplayMusic;
    }
}