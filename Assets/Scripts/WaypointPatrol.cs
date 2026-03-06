using UnityEngine.AI;
using UnityEngine;

public class WaypointPatrol : MonoBehaviour
{
    [SerializeField]
    Transform[] waypoints;
    NavMeshAgent agent;

    int currentWaypointIndex;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        agent.SetDestination(waypoints[0].position);
    }

    void Update()
    {
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }
}
