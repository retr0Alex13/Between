using UnityEngine;

namespace Between.Player
{
    public class FootstepPlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioClip[] _footSteps;

        [SerializeField]
        private FirstPersonController _player;

        [SerializeField]
        private float _footstepTime;
        private float _timer;

        private void Start()
        {
            _player.OnPlayerWalk += OnPlayerWalk;
            _player.OnPlayerStop += OnPlayerStop;
        }

        private void OnPlayerWalk()
        {
            _timer += Time.deltaTime;
            if (_timer >= _footstepTime)
            {
                int randomIndex = Random.Range(0, _footSteps.Length);
                AudioManager.Instance.PlaySound(_footSteps[randomIndex]);

                _timer = 0;
            }
        }

        private void OnPlayerStop()
        {
            Debug.Log("Player Stopped");
        }
    }
}