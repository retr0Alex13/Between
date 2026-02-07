using UnityEngine;

namespace Between.Data
{
    public class LevelRoot : MonoBehaviour
    {
        [SerializeField]
        private Transform _playerSpawnPoint;

        public Transform PlayerSpawnPoint => _playerSpawnPoint;
    }
}
