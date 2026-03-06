using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameView : MonoBehaviour
{
    [SerializeField]
    float exitTime = 4f;
    float timer;

    float desiredAlpha = 1;
    float currentAlpha;

    bool isTriggered;

    CanvasGroup canvasGroup;

    private void OnEnable()
    {
        EndGame.OnGameOver += SetGameOverBool;
    }

    private void OnDisable()
    {
        EndGame.OnGameOver -= SetGameOverBool;
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void SetGameOverBool()
    {
        isTriggered = true;
    }

    void Update()
    {
        if (isTriggered)
        {
            ShowGameOverScreen();
            WaitSecondsForExitFromGame();
        }
    }

    void WaitSecondsForExitFromGame()
    {
        timer += Time.deltaTime;

        if (timer > exitTime)
        {
            timer = exitTime;
            Application.Quit();
        }
    }

    public void ShowGameOverScreen()
    {
        currentAlpha = Mathf.MoveTowards(currentAlpha, desiredAlpha, 2.0f * Time.deltaTime);
        canvasGroup.alpha = currentAlpha;
    }
}
