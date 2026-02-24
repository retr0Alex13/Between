using Between.Level;
using Between.Player;
using UnityEngine;

namespace Between.Data
{
    [CreateAssetMenu(fileName = "ObjectsData", menuName = "Scriptable Objects/ObjectsData")]
    public class GameObjectsData : ScriptableObject
    {
        [SerializeField]
        private FirstPersonController _playerPrefab;

        [SerializeField]
        private LevelRoot[] _levels;

        public LevelRoot[] Levels => _levels;

        public FirstPersonController PlayerPrefab => _playerPrefab;
    }
}
