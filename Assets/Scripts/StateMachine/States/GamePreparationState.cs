using Between.Player;
using UnityEngine;
using Void.StateMachines;

namespace Between.StateMachines
{
    public class GamePreparationState : IState
    {
        private readonly PlayerMovement _playerMovement;

        public GamePreparationState(PlayerMovement playerMovement)
        {
            _playerMovement = playerMovement;
        }

        public void Enter()
        {

        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }
}
