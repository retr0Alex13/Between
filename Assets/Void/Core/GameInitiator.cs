using System.Collections;
using UnityEngine;
using Void.StateMachines;

namespace Void.Core
{
    public class GameInitiator : MonoBehaviour
    {
        private IEnumerator Start()
        {
            StateMachine stateMachine = new StateMachine();

            yield return null;
        }
    }
}
