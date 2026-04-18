using UnityEngine;

namespace Between.Data
{
    [CreateAssetMenu(fileName = "GameAudioData", menuName = "Scriptable Objects/GameAudioData")]
    public class GameAudioData : ScriptableObject
    {
        [SerializeField]
        private AudioClip[] _footstepAudioClips;

        [SerializeField]
        private AudioClip _spookyGameplayMusic;

        public AudioClip[] FootstepAudioClips => _footstepAudioClips;
        public AudioClip SpookyGameplayMusic => _spookyGameplayMusic;
    }
}