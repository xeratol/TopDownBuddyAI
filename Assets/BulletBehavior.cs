using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private Rigidbody _body;

    public float forwardSpeed = 10;
    public float lifetime = 2;
    public float damage = 10;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        Debug.Assert(_body, "No Rigidbody assigned", this);

        _body.velocity = transform.forward * forwardSpeed;

        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var otherHealth = collision.gameObject.GetComponent<HealthBehavior>();
        if (otherHealth)
        {
            otherHealth.health -= damage;
        }

        Destroy(gameObject);
    }
}
