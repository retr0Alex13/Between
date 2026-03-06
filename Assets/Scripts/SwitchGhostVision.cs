using UnityEngine.InputSystem;
using UnityEngine;

public class SwitchGhostVision : MonoBehaviour
{
    [SerializeField]
    GhostVisionController ghostVision;

    CharacterController characterController;

    [SerializeField]
    float timeToActivateVision = 1f;
    float timer;

    bool isStandignStill;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleGhostVision();
    }

    private void HandleGhostVision()
    {
        if (characterController.velocity.magnitude == 0)
        {
            timer += Time.deltaTime;

            if (timer > timeToActivateVision)
            {
                timer = timeToActivateVision;
                ghostVision.SwitchGhostVision(true);
            }
        }
        else
        {
            timer = 0;
            ghostVision.SwitchGhostVision(false);
        }
    }
}
