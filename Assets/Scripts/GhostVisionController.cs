using UnityEngine;

public class GhostVisionController : MonoBehaviour
{
    Camera playerCamera;

    void Awake()
    {
        playerCamera = GetComponent<Camera>();
    }

    void Update()
    {

    }

    public void SwitchGhostVision(bool status)
    {
        SwitchGhostLayer(status);
    }

    void SwitchGhostLayer(bool visionStatus)
    {
        int currentCullingMask = playerCamera.cullingMask;
        int ghostLayer = LayerMask.NameToLayer("Ghost Object");
        int newCullingMask;

        if (visionStatus)
        {
            newCullingMask = currentCullingMask | (1 << ghostLayer);
        }
        else
        {
            newCullingMask = currentCullingMask & ~(1 << ghostLayer);
        }

        playerCamera.cullingMask = newCullingMask;
    }
}
