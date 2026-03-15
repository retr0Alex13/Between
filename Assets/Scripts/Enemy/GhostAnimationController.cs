using UnityEngine;

public class GhostAnimationController : MonoBehaviour
{
    [SerializeField]
    private bool _walkingGhost;

    [SerializeField]
    private Animator _animator;

    private void Start()
    {
        _animator.SetBool("isWalking", _walkingGhost);
    }
}
