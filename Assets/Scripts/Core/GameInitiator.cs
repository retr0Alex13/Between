using Between.Player;
using Between.StateMachines;
using Between.Data;
using System.Collections;
using UnityEngine;

namespace Between.Core
{
    public class GameInitiator : MonoBehaviour
    {
        [SerializeField]
        private GameObjectsData _gameObjectsData;

        [SerializeField]
        private GameConfigData _gameConfigData;

        private StateMachine _stateMachine;

        private IEnumerator Start()
        {
            _stateMachine = new StateMachine(_gameObjectsData, _gameConfigData);
            _stateMachine.Initialize(_stateMachine.GamePreparationState);

            yield return null;
        }

        private void Update()
        {
            _stateMachine.Execute();
        }
    }
}
