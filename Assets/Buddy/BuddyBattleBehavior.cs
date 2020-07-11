using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuddyBattleBehavior : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.AI.NavMeshAgent _agent = null;

    [SerializeField]
    private Transform _player = null;

    [SerializeField]
    private PlayerInfo _playerInfo = null;

    void Start()
    {
        
    }
}
