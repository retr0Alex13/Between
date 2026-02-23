using Between.Player;
using System;
using UnityEngine;

namespace Between
{
    public class LevelFinishTrigger : MonoBehaviour
    {
        public event Action OnPlayerReachedFinish;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out FirstPersonController player))
            {
                OnPlayerReachedFinish?.Invoke();
            }
        }
    }
}
