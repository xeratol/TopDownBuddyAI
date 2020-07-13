using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _agent = null;

    [SerializeField]
    private GunBehavior _gun = null;

    private Transform _player;
    private Vector3 _lastKnownPlayerPosition;

    void Start()
    {
        Debug.Assert(_agent, "Agent not set", this);
        Debug.Assert(_gun, "Gun not set", this);
        Debug.Assert(PlayerController.instance, "No player exists", this);

        _player = PlayerController.instance.transform;
        _lastKnownPlayerPosition = transform.position;
    }

    void Update()
    {
        if ((_lastKnownPlayerPosition - _player.position).sqrMagnitude > 1.0f)
        {
            _lastKnownPlayerPosition = _player.position;
            _agent.SetDestination(_lastKnownPlayerPosition);
        }

        if (IsPlayerVisibile())
        {
            _gun.Fire();
            _agent.isStopped = true;

            var desiredRot = Quaternion.LookRotation(_player.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, Time.deltaTime * _agent.angularSpeed);
        }
        else
        {
            _agent.isStopped = false;
        }
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
    }
}
