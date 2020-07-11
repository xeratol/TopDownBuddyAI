using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehavior : MonoBehaviour
{
    public Transform bulletPrefab = null;
    [Tooltip("Bullets per second"), Min(0.01f)]
    public float fireRate = 1;
    public float spreadDegrees = 5;

    private float _delayBetweenFire = 0;
    private float _timeLastFired = 0;

    void Start()
    {
        Debug.Assert(bulletPrefab, "Bullet Prefab not set", this);
        Debug.Assert(fireRate > 0.01f, "Invalid Fire Rate", this);

        _delayBetweenFire = 1 / fireRate;
    }

    public void Fire()
    {
        if (_timeLastFired + _delayBetweenFire < Time.time)
        {
            _timeLastFired = Time.time;
            var rotation = transform.rotation;
            var rotationModifier = Quaternion.AngleAxis(Random.Range(-spreadDegrees, spreadDegrees), Vector3.up);

            var bullet = Instantiate(bulletPrefab, transform.position, rotation * rotationModifier);
            var bulletCollider = bullet.GetComponent<Collider>();
            Physics.IgnoreCollision(GetComponent<Collider>(), bulletCollider);
        }
    }
}
