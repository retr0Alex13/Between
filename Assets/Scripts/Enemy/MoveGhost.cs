using UnityEngine;
using System;

public class MoveGhost : MonoBehaviour
{
    public enum EndBehavior { Loop, PingPong }

    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _damping = 2f;
    [SerializeField] private float _arrivalDistance = 0.001f;
    [SerializeField] private EndBehavior _endBehavior = EndBehavior.PingPong;

    private int _currentWaypointIndex = 0;
    private int _direction = 1;

    private void Start()
    {
        if (_waypoints.Length > 0)
        {
            _currentWaypointIndex = 0;
            _direction = 1;
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
        switch (_endBehavior)
        {
            case EndBehavior.Loop:
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                break;

            case EndBehavior.PingPong:
                _currentWaypointIndex += _direction;

                if (_currentWaypointIndex >= _waypoints.Length)
                {
                    _direction = -1;
                    _currentWaypointIndex = _waypoints.Length - 2;
                }
                else if (_currentWaypointIndex < 0)
                {
                    _direction = 1;
                    _currentWaypointIndex = 1;
                }
                break;
        }
    }
}