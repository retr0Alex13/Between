using Between.View;
using UnityEngine;

namespace Between.Data
{
    [CreateAssetMenu(fileName = "ViewPrefabsData", menuName = "Scriptable Objects/ViewPrefabsData")]
    public class ViewPrefabsData : ScriptableObject
    {
        [SerializeField]
        private GameplayView _gameplayView;

        [SerializeField]
        private PauseView _pauseView;

        public GameplayView GameplayView => _gameplayView;
        public PauseView PauseView => _pauseView;
    }
}
