using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _agent = null;

    public float maxDistanceToHide = 0.5f;
    public bool IsHiding { get; private set; } = false;

    public float idleTime = 3.0f;
    private float _lastMovementTime = 0;
    public bool IsIdle { get; private set; } = false;

    public bool IsMoving { get; private set; } = false;

    private Vector3 _lastPosition;

    void Start()
    {
        Debug.Assert(_agent, "Agent not set", this);

        _lastMovementTime = Time.time;
        _lastPosition = transform.position;
    }

    void Update()
    {
        UpdateMoving();
        UpdateHiding();
        UpdateIdle();
    }

    private void UpdateMoving()
    {
        IsMoving = (_lastPosition - transform.position).sqrMagnitude > 0.001f;
    }

    private void UpdateIdle()
    {
        if (IsMoving)
        {
            _lastMovementTime = Time.time;
            IsIdle = false;
        }
        else if (Time.time - _lastMovementTime > idleTime)
        {
            IsIdle = true;
        }
        _lastPosition = transform.position;
    }

    private void UpdateHiding()
    {
        NavMeshHit hit;
        IsHiding = _agent.FindClosestEdge(out hit) && hit.distance < maxDistanceToHide;
    }
}
