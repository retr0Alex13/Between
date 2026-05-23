using Between.Inputs;
using UnityEngine;

namespace Between.Player
{
    [RequireComponent(typeof(InputReader))]
    public class PlayerLook : MonoBehaviour
    {
        [SerializeField]
        private float _mouseSensitivity = 20f;

        [SerializeField]
        private float maxLookAngle = 90f;

        [SerializeField]
        private float minLookAngle = -90f;

        [SerializeField]
        private Transform _cameraRoot;

        private float _xRotation;

        public void Look(float inputX, float inputY)
        {
            if (Time.deltaTime == 0)
                return;

            float mouseX = inputX * _mouseSensitivity;
            float mouseY = inputY * _mouseSensitivity;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, minLookAngle, maxLookAngle);

            _cameraRoot.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
    }
}
