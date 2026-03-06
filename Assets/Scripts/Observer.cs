using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Observer : MonoBehaviour
    {
        Transform player;
        bool isPlayerInRange;

        void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent(out PlayerBehavior player))
            {
                isPlayerInRange = true;
                this.player = player.transform;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.transform.TryGetComponent(out PlayerBehavior player))
            {
                isPlayerInRange = false;
                this.player = null;
            }
        }

        void Update()
        {
            if (isPlayerInRange)
            {
                if (player == null)
                    return;

                Vector3 direction = player.position - transform.position + Vector3.up;
                Ray ray = new Ray(transform.position, direction);
                RaycastHit raycastHit;

                if (Physics.Raycast(ray, out raycastHit))
                {
                    if (raycastHit.transform.TryGetComponent(out PlayerBehavior player))
                    {
                        player.KillPLayer();
                    }
                }
            }
        }
    }
}