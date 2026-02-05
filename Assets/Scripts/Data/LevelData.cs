using UnityEngine;

namespace Between.Data
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
    public class LevelData : ScriptableObject
    {
        [SerializeField]
        private LevelRoot[] _levels;
    }
}
