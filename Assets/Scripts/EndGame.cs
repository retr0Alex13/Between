using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public delegate void EndGameAction();
    public static event EndGameAction OnGameOver;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out PlayerBehavior player))
        {
            OnGameOver?.Invoke();
        }
    }
}
