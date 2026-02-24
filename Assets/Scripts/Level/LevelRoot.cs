using UnityEngine;

namespace Between.Level
{
    public class LevelRoot : MonoBehaviour
    {
        [SerializeField]
        private Transform _playerSpawnPoint;

        [SerializeField]
        private GhostObject[] _ghostObjects;

        [SerializeField]
        private LevelFinishTrigger _levelFinish;

        public GhostObject[] GhostObjects => _ghostObjects;
        public Transform PlayerSpawnPoint => _playerSpawnPoint;
        public LevelFinishTrigger LevelFinish => _levelFinish;
    }
}
