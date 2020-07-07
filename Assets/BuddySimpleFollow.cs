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

    public float distanceFromPlayer = 4;

    private Vector3 _destination;

    void Start()
    {
        Debug.Assert(_agent, "Agent not set", this);
        Debug.Assert(_player, "Player not set", this);
        Debug.Assert(_playerInfo, "Player Info not set", this);
    }

    void Update()
    {
        if (_playerInfo.IsIdle)
        {
            return;
        }

        var destinationUpdated = false;
        var target = _player.position + (transform.position - _player.position).normalized * distanceFromPlayer;
        if ((target - _destination).sqrMagnitude > 0.1f)
        {
            _destination = target;
            destinationUpdated = true;
        }

        if (destinationUpdated)
        {
            _agent.SetDestination(_destination);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(_destination, Vector3.one * 0.5f);
    }
}
