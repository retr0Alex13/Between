using Between.Player;
using UnityEngine;

namespace Between.Data
{
    [CreateAssetMenu(fileName = "ObjectsData", menuName = "Scriptable Objects/ObjectsData")]
    public class GameObjectsData : ScriptableObject
    {
        [SerializeField]
        private PlayerMovement _playerPrefab;

        public PlayerMovement PlayerPrefab => _playerPrefab;
    }
}
