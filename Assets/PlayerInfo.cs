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

    void Start()
    {
        Debug.Assert(_agent, "Agent not set", this);
    }

    void Update()
    {
        NavMeshHit hit;
        IsHiding = _agent.FindClosestEdge(out hit) && hit.distance < maxDistanceToHide;
    }
}
