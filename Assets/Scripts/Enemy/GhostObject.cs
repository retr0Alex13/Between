using Between.Player;
using System;
using UnityEngine;

namespace Between
{
    public class GhostObject : MonoBehaviour
    {
        public event Action OnPlayerWalkedThrough;

        [SerializeField]
        private bool _isTutorial;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out FirstPersonController player) && !_isTutorial)
            {
                OnPlayerWalkedThrough?.Invoke();
            }
        }
    }
}
