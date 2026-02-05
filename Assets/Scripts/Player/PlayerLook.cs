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
            float mouseX = inputX * _mouseSensitivity * Time.deltaTime;
            float mouseY = inputY * _mouseSensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, minLookAngle, maxLookAngle);

            _cameraRoot.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
    }
}
