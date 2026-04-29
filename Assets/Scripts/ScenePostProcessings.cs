using UnityEngine;
using UnityEngine.Rendering;

namespace Between
{
    public class ScenePostProcessings : MonoBehaviour
    {
        [SerializeField]
        private Volume _originalVolume;

        [SerializeField]
        private Volume _standingStillVolume;

        public Volume OriginalVolume => _originalVolume;
        public Volume StandingStillVolume => _standingStillVolume;
    }
}