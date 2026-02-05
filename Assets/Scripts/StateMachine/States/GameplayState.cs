using Between.Player;
using Void.StateMachines;

namespace Between.StateMachines
{
    public class GameplayState : IState
    {
        private readonly PlayerMovement _playerMovement;

        public GameplayState(PlayerMovement playerMovement)
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
