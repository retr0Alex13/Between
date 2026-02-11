using UnityEngine;

namespace Between.Data
{
    public class LevelRoot : MonoBehaviour
    {
        [SerializeField]
        private Transform _playerSpawnPoint;

        [SerializeField]
        private GameObject[] _ghostObjects;

        public GameObject[] GhostObjects => _ghostObjects;
        public Transform PlayerSpawnPoint => _playerSpawnPoint;
    }
}
