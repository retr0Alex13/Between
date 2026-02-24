using Between.Data;
using Between.StateMachines;
using Between.View;
using System.Collections;
using UnityEngine;

namespace Between.Core
{
    public class GameInitiator : MonoBehaviour
    {
        [SerializeField]
        private GameObjectsData _gameObjectsData;

        [SerializeField]
        private ViewPrefabsData _viewPrefabsData;

        [SerializeField]
        private GameConfigData _gameConfigData;

        [SerializeField]
        private ViewManager _viewManager;

        private StateMachine _stateMachine;

        private IEnumerator Start()
        {
            _stateMachine = new StateMachine(_viewManager, _viewPrefabsData, _gameObjectsData, _gameConfigData);
            _stateMachine.Initialize(_stateMachine.GamePreparationState);

            yield return null;
        }

        private void Update()
        {
            _stateMachine.Execute();
        }
    }
}
