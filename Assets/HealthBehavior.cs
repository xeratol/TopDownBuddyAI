using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehavior : MonoBehaviour
{
    public float maxHealth = 100;

    private float _health = 100;
    public float health
    {
        get { return _health; }
        set
        {
            _health = value;
            if (_health < 0 )
            {
                _health = 0;
                OnDeathListener?.Invoke();
            }
            else if (_health > maxHealth)
            {
                _health = maxHealth;
            }
        }
    }

    public event Action OnDeathListener;

    void Start()
    {
        _health = maxHealth;
    }


}
