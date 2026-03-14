using UnityEngine;
using System;

public class MoveGhost : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _damping = 2f;
    [SerializeField] private float _arrivalDistance = 0.001f;

    private int _currentWaypointIndex = 0;

    private void Start()
    {
        if (_waypoints.Length > 0)
        {
            _currentWaypointIndex = 0;
        }
    }

    private void Update()
    {
        if (_waypoints == null || _waypoints.Length == 0)
        {
            Debug.LogWarning("Waypoints is not assigned!");
            return;
        }

        Transform target = _waypoints[_currentWaypointIndex];

        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);

        float step = _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance < _arrivalDistance)
        {
            SetNextWaypoint();
        }

        var rotation = Quaternion.LookRotation(target.position - transform.position);
        rotation.x = 0;
        rotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _damping);
    }

    private void SetNextWaypoint()
    {
        _currentWaypointIndex++;

        if (_currentWaypointIndex >= _waypoints.Length)
        {
            Array.Reverse(_waypoints);
            _currentWaypointIndex = 1;
        }
    }
}