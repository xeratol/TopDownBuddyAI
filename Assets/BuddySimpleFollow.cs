using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuddySimpleFollow : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _agent = null;

    [SerializeField]
    private Transform _player = null;

    [SerializeField]
    private PlayerInfo _playerInfo = null;

    public float followingDistance = 4;

    public float normalSpeed = 5;
    public float lostSpeed = 8;

    [Tooltip("Distance to wander around from current position")]
    public float wanderDistance = 1.0f;
    [Tooltip("Minimum distance from player while wandering")]
    public float wanderMinDistance = 2.0f;
    [Tooltip("Maximum distance from player while wandering")]
    public float wanderMaxDistance = 6.0f;
    [Tooltip("Minimum time between wander")]
    public float wanderMinIntervalTime = 1.0f;
    [Tooltip("Maximum time between wander")]
    public float wanderMaxIntervalTime = 4.0f;
    private float _lastWanderTimer = 0;

    private Vector3 _lastKnownPlayerPosition;
    private Vector3 _destination;

    private enum BuddyState
    {
        Follow,
        Wander,
        Lost,
    }
    private BuddyState _state = BuddyState.Follow;

    void Start()
    {
        Debug.Assert(_agent, "Agent not set", this);
        Debug.Assert(_player, "Player not set", this);
        Debug.Assert(_playerInfo, "Player Info not set", this);

        _lastKnownPlayerPosition = _player.position;
    }

    void Update()
    {
        if (IsPlayerVisibile())
        {
            _agent.speed = normalSpeed;
            _lastKnownPlayerPosition = _player.position;

            if (_playerInfo.IsIdle && _state != BuddyState.Lost)
            {
                if (_agent.remainingDistance < 0.1f && _lastWanderTimer <= 0)
                {
                    var target = GetWanderDestination();
                    if (IsValidWanderDestination(target))
                    {
                        _destination = target;
                        _agent.SetDestination(_destination);
                        _lastWanderTimer = Random.Range(wanderMinIntervalTime, wanderMaxIntervalTime);
                        _state = BuddyState.Wander;
                    }
                }
                _lastWanderTimer -= Time.deltaTime;
            }
            else //if (_playerInfo.IsMoving)
            {
                _lastWanderTimer = 0;
                var target = GetFollowDestination();
                if (IsValidFollowDestination(target))
                {
                    _destination = target;
                    _agent.SetDestination(_destination);
                    _state = BuddyState.Follow;
                }
            }
        }
        else
        {
            _agent.speed = lostSpeed;

            if (_state != BuddyState.Lost)
            {
                _destination = _lastKnownPlayerPosition;
                _agent.SetDestination(_destination);
                _state = BuddyState.Lost;
            }
            else if (_agent.remainingDistance < followingDistance * 0.5f)
            {
                _destination = _player.position;
                _agent.SetDestination(_destination);
            }
        }
    }

    private Vector3 GetFollowDestination()
    {
        var target = _player.position + (transform.position - _player.position).normalized * followingDistance;

        NavMeshHit hit;
        return _agent.Raycast(target, out hit) ? hit.position : target;
    }

    private bool IsValidFollowDestination(Vector3 target)
    {
        return (target - _destination).sqrMagnitude > 0.1f;
    }

    private Vector3 GetWanderDestination()
    {
        var randomAngle = Random.Range(0, Mathf.PI * 2);
        var target = transform.position + new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)) * wanderDistance;

        NavMeshHit hit;
        return _agent.Raycast(target, out hit) ? hit.position : target;
    }

    private bool IsValidWanderDestination(Vector3 destination)
    {
        NavMeshHit hit;
        var blocked = NavMesh.Raycast(_player.position, destination, out hit, NavMesh.AllAreas);
        if (blocked)
        {
            return false;
        }

        var distanceToPlayerSq = (destination - _player.position).sqrMagnitude;
        return distanceToPlayerSq >= wanderMinDistance * wanderMinDistance &&
            distanceToPlayerSq <= wanderMaxDistance * wanderMaxDistance;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(_destination, Vector3.one * 0.5f);
    }

    private bool IsPlayerVisibile()
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position, _player.position, out hit, LayerMask.GetMask("Wall")))
        {
            Debug.DrawLine(transform.position, hit.point);
            return false;
        }
        return true;

        //NavMeshHit hit;
        //var visible =  !_agent.Raycast(_player.position, out hit);
        //return visible || (hit.position - _player.position).sqrMagnitude <= 2.0f;
    }
}
