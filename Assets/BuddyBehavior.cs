using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuddyBehavior : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _agent = null;

    [SerializeField]
    private Transform _player = null;

    [SerializeField]
    private PlayerInfo _playerInfo = null;

    public float minDistance = 2;
    public float maxDistance = 4;

    private Vector3 _destination;

    private Vector3 _lastRecordedPlayerPosition;

    void Start()
    {
        Debug.Assert(_agent, "Agent not set", this);
        Debug.Assert(_player, "Player not set", this);
        Debug.Assert(_playerInfo, "Player Info not set", this);

        _destination = _player.position;
        _lastRecordedPlayerPosition = _player.position;
    }

    void Update()
    {
        if (_playerInfo.IsHiding)
        {
            NavMeshHit hit;
            if (_agent.FindClosestEdge(out hit))
            {
                _destination = hit.position;
                _agent.SetDestination(_destination);
            }
        }
        else
        {
            var distanceToPlayerSq = (_player.position - _destination).sqrMagnitude;
            if (distanceToPlayerSq > maxDistance * maxDistance || distanceToPlayerSq < minDistance * minDistance)
            {
                //MoveToRandomPointNearPlayer();
                FollowPlayer();
            }
        }
    }

    private void FollowPlayer()
    {

    }

    private void MoveToRandomPointNearPlayer()
    {
        var randomDistance = Random.Range(minDistance, maxDistance);
        var randomAngle = Random.Range(Mathf.PI * 0.25f, Mathf.PI * 0.75f);

        var forwardAxis = transform.position - _player.position;
        forwardAxis.Normalize();
        var rightAxis = Vector3.Cross(Vector3.up, forwardAxis);

        var direction = forwardAxis * Mathf.Sin(randomAngle) + rightAxis * Mathf.Cos(randomAngle);
        _destination = _player.position + direction * randomDistance;
        _agent.SetDestination(_destination);
    }

    private void OnDrawGizmosSelected()
    {
        var angles = new List<float>();
        for (var i = 0; i <= 16; ++i)
        {
            var angle = i * Mathf.PI * 0.125f;
            angles.Add(angle);
        }

        for (var i = 0; i < 16; ++i)
        {
            var startPoint = new Vector3(Mathf.Cos(angles[i]), 0, Mathf.Sin(angles[i]));
            var endPoint = new Vector3(Mathf.Cos(angles[i + 1]), 0, Mathf.Sin(angles[i + 1]));

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_player.position + startPoint * minDistance, _player.position + endPoint * minDistance);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_player.position + startPoint * maxDistance, _player.position + endPoint * maxDistance);
        }

        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(_destination, Vector3.one * 0.5f);
    }
}
