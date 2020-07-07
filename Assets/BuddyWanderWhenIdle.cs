using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuddyWanderWhenIdle : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _agent = null;

    [SerializeField]
    private Transform _player = null;

    [SerializeField]
    private PlayerInfo _playerInfo = null;

    [Tooltip("Distance to wander around from current position")]
    public float wanderDistance = 1.0f;

    [Tooltip("Minimum distance from player")]
    public float minDistance = 2.0f;

    [Tooltip("Maximum distance from player")]
    public float maxDistance = 6.0f;

    public float minIntervalTime = 1.0f;
    public float maxIntervalTime = 4.0f;
    private float _lastWanderTimer = 0;

    private bool _isIdle = false;

    void Start()
    {
        Debug.Assert(_agent, "Agent not set", this);
        Debug.Assert(_player, "Player not set", this);
        Debug.Assert(_playerInfo, "Player Info not set", this);
    }

    void Update()
    {
        if (_playerInfo.IsIdle && _agent.velocity.sqrMagnitude < 0.001f && _lastWanderTimer <= 0)
        {
            var randomAngle = Random.Range(0, Mathf.PI * 2);
            var destination = transform.position + new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)) * wanderDistance;
            var distanceToPlayerSq = (destination - _player.position).sqrMagnitude;
            if (distanceToPlayerSq >= minDistance * minDistance &&
                distanceToPlayerSq <= maxDistance * maxDistance)
            {
                _agent.SetDestination(destination);
                _lastWanderTimer = Random.Range(minIntervalTime, maxIntervalTime);
            }
        }
        _lastWanderTimer -= Time.deltaTime;
    }
}
