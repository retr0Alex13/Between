using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupScreenView : MonoBehaviour
{
    [SerializeField]
    float startUpScreenTime = 5f;
    float timer;

    float desiredAlpha = 0f;
    float currentAlpha;

    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > startUpScreenTime)
        {
            timer = startUpScreenTime;
            FadeOutScreen();
        }
    }

    private void FadeOutScreen()
    {
        currentAlpha = Mathf.MoveTowards(currentAlpha, desiredAlpha, 2.0f * Time.deltaTime);
        canvasGroup.alpha = currentAlpha;
    }
}
