using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
    [SerializeField]
    private HealthBehavior _healthBehavior = null;

    void Start()
    {
        Debug.Assert(_healthBehavior, "Health Behavior not set", this);

        _healthBehavior.OnDeathListener += OnDeath;
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }
}
