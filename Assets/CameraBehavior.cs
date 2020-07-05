using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField]
    private Transform _player = null;

    private Vector3 _relPosition;

    void Start()
    {
        Debug.Assert(_player, "Player not set", this);

        _relPosition = transform.position - _player.position;
    }

    void LateUpdate()
    {
        transform.position = _player.position + _relPosition;
    }
}
