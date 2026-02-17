using Between.Player;
using System;
using UnityEngine;

namespace Between
{
    public class GhostObject : MonoBehaviour
    {
        public event Action OnPlayerWalkedThrough;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out FirstPersonController player))
            {
                OnPlayerWalkedThrough?.Invoke();
            }
        }
    }
}
