using Between.Inputs;
using System;
using UnityEngine;

namespace Between.Player
{
    [RequireComponent(typeof(InputReader))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private float _moveSpeed = 10f;

        [SerializeField]
        private float _gravity = -9.81f;

        [SerializeField]
        private float _groundRadius = 0.4f;

        [SerializeField]
        private float _jumpHeight = 1f;

        [SerializeField]
        private LayerMask _groundMask;

        [SerializeField]
        private Transform _groundCheck;

        private Vector3 _velocity;
        private bool _isGrounded;

        private CharacterController _controller;

        public CharacterController CharacterController => _controller;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        public void HandleGravity()
        {
            _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundRadius, _groundMask);

            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            _velocity.y += _gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }

        public void Move(float inputX, float inputY)
        {
            float moveX = inputX;
            float moveZ = inputY;

            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            _controller.Move(move * _moveSpeed * Time.deltaTime);
        }

        public void HandleJump()
        {
            if (_isGrounded)
            {
                _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            }
        }
    }
}
