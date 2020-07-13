using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

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

            if (_image)
            {
                _image.fillAmount = _health / maxHealth;
            }
        }
    }

    public Image _image = null;

    public event Action OnDeathListener;

    void Start()
    {
        _health = maxHealth;
    }


}
