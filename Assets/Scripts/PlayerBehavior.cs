using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField]
    FirstPersonController firstPersonController;

    [SerializeField]
    StarterAssetsInputs starterAssetsInputs;

    public void KillPLayer()
    {
        DisablePlayerControls();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // It's should not be here
                                                                          // But since we don't use it anywhere I just put it here
    }

    private void DisablePlayerControls()
    {
        firstPersonController.enabled = false;
        starterAssetsInputs.enabled = false;
    }
}
