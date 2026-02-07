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

        private IEnumerator Start()
        {
            StateMachine stateMachine = new StateMachine(_gameObjectsData);
            stateMachine.Initialize(stateMachine.GamePreparationState);

            yield return null;
        }
    }
}
