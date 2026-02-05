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

        private PlayerMovement _playerPrefab;

        private IEnumerator Start()
        {
            _playerPrefab = _gameObjectsData.PlayerPrefab;

            StateMachine stateMachine = new StateMachine(_playerPrefab);
            stateMachine.Initialize(stateMachine.GameplayState);

            yield return null;
        }
    }
}
