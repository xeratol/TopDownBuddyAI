using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuddyBattleBehavior : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _agent = null;

    [SerializeField]
    private Transform _player = null;

    [SerializeField]
    private PlayerInfo _playerInfo = null;

    [Min(0.5f)]
    public float scanDistance = 5.0f;

    [Min(1.0f)]
    public float minDistanceToPlayer = 2.0f;
    [Min(1.0f)]
    public float maxDistanceToPlayer = 8.0f;

    [Min(1.0f)]
    public float healAmount = 50.0f;

    private Vector3 _lastTargetPlayerPosition;

    readonly Vector3[] relTargetPos = 
    {
        new Vector3(0, 0, 1.0f),
        new Vector3(0.7071f, 0, 0.7071f),
        new Vector3(1.0f, 0, 0),
        new Vector3(0.7071f, 0, -0.7071f),
        new Vector3(0, 0, -1.0f),
        new Vector3(-0.7071f, 0, -0.7071f),
        new Vector3(-1.0f, 0, 0),
        new Vector3(-0.7071f, 0, 0.7071f),
    };

    private enum BuddyState
    {
        Hiding,
        Healing,
        Attacking,
    }
    private BuddyState _state = BuddyState.Hiding;

    void Start()
    {
        Debug.Assert(_agent, "Agent not set", this);
        Debug.Assert(_player, "Player not set", this);
        Debug.Assert(_playerInfo, "Player Info not set", this);

        _lastTargetPlayerPosition = _player.position;
    }

    private void Update()
    {
        if (_state == BuddyState.Healing)
        {
            if (!_playerInfo.IsCriticalHealth)
            {
                _state = BuddyState.Hiding;
            }
            else
            {
                if ((_player.position - _lastTargetPlayerPosition).sqrMagnitude > 0.1f)
                {
                    _lastTargetPlayerPosition = _player.position;
                    _agent.SetDestination(_lastTargetPlayerPosition);
                }

                var distSq = (_player.position - transform.position).sqrMagnitude;
                if (distSq < minDistanceToPlayer * minDistanceToPlayer)
                {
                    var playerHealth = _player.GetComponent<HealthBehavior>();
                    playerHealth.health += healAmount;

                    FindNewPosition();
                }
            }
        }
        else if (_state == BuddyState.Hiding)
        {
            if (_playerInfo.IsCriticalHealth)
            {
                _state = BuddyState.Healing;
                _lastTargetPlayerPosition = _player.position;
                _agent.SetDestination(_lastTargetPlayerPosition);
            }
            else if(!IsWithinProximityOfPlayer(transform.position))
            {
                _lastTargetPlayerPosition = _player.position;
                FindNewPosition();
            }
        }
    }

    private void FindNewPosition()
    {
        var possiblePositions = ScanAreaAroundPlayer();
        if (possiblePositions.Count > 0)
        {
            var destination = FindClosestPosition(possiblePositions, transform.position);
            _agent.SetDestination(destination);
        }
    }

    List<Vector3> ScanAreaAroundPlayer()
    {
        var possiblePositions = new List<Vector3>();
        foreach (var relPos in relTargetPos)
        {
            var targetPos = _player.position + relPos * scanDistance;
            NavMeshHit hit;

            var scanHitSomething = NavMesh.Raycast(_player.position, targetPos, out hit, NavMesh.AllAreas);
            var scanHistDistanceAllowed = scanHitSomething && IsWithinProximityOfPlayer(hit.position);

            if (scanHistDistanceAllowed)
            {
                Debug.DrawLine(_player.position, hit.position);
                possiblePositions.Add(hit.position);
            }
            else if (scanHitSomething && !scanHistDistanceAllowed)
            {
                continue;
            }
            else if (NavMesh.FindClosestEdge(targetPos, out hit, NavMesh.AllAreas) &&
                IsWithinProximityOfPlayer(hit.position) &&
                Vector3.Dot(_player.position - hit.position, relPos) < 0)
            {
                Debug.DrawLine(_player.position, targetPos, Color.blue);
                Debug.DrawLine(targetPos, hit.position, Color.yellow);
                possiblePositions.Add(hit.position);
            }
        }
        return possiblePositions;
    }

    bool IsWithinProximityOfPlayer(Vector3 pos)
    {
        var distSq = (_player.position - pos).sqrMagnitude;
        return distSq > minDistanceToPlayer * minDistanceToPlayer &&
            distSq < maxDistanceToPlayer * maxDistanceToPlayer;
    }

    Vector3 FindClosestPosition(List<Vector3> positions, Vector3 target)
    {
        Debug.Assert(positions.Count > 0, "Invalid parameter", this);

        var minDistSq = float.MaxValue;
        var closestPos = Vector3.zero;
        foreach (var pos in positions)
        {
            var distSq = (pos - target).sqrMagnitude;
            if (distSq < minDistSq)
            {
                minDistSq = distSq;
                closestPos = pos;
            }
        }

        return closestPos;
    }
}
