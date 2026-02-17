using Between.Inputs;
using UnityEngine;

namespace Between.Player
{
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField]
        private bool _canMove = true;

        [SerializeField]
        private bool _canLook = true;

        [SerializeField]
        private bool _canJump = true;

        [SerializeField]
        private PlayerMovement _playerMovement;

        [SerializeField]
        private PlayerLook _playerLook;

        private InputReader _input;

        private void Awake()
        {
            _input = GetComponent<InputReader>();
            _input.JumpEvent += _playerMovement.HandleJump;
        }

        private void Start()
        {
            _playerMovement.SetCanJump(_canJump);
        }

        private void Update()
        {
            _playerMovement.HandleGravity();

            if (_canMove)
            {
                _playerMovement.Move(_input.MoveInput.x, _input.MoveInput.y);
            }

            if (_canLook)
            {
                _playerLook.Look(_input.LookInput.x, _input.LookInput.y);
            }
        }

        private void OnDisable()
        {
            _input.JumpEvent -= _playerMovement.HandleJump;
        }

        public void SetMoveAbility(bool canMove)
        {
            _canMove = canMove;
        }

        public void SetLookAbility(bool canLook)
        {
            _canLook = canLook;
        }

        public void SetJumpAbility(bool canJump)
        {
            _canJump = canJump;
            _playerMovement.SetCanJump(canJump);
        }

        public float GetPlayerVelocity()
        {
            return _playerMovement.CharacterController.velocity.magnitude;
        }
    }
}
