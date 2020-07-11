using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _agent = null;

    [SerializeField]
    private GunBehavior _gun = null;

    void Start()
    {
        Debug.Assert(_agent, "Agent not set", this);
        Debug.Assert(_gun, "Gun not set", this);
    }

    void Update()
    {
        UpdatePosition();
        UpdateRotation();

        if (Input.GetMouseButton(0))
        {
            _gun.Fire();
        }
    }

    private void UpdateRotation()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));
        var t = 0.0f;
        if (plane.Raycast(ray, out t))
        {
            var point = ray.GetPoint(t);
            var desiredLook = Quaternion.LookRotation(point - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredLook, _agent.angularSpeed * Time.deltaTime);
        }
    }

    private void UpdatePosition()
    {
        var desiredMovement = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            desiredMovement += Vector3.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            desiredMovement += Vector3.right;
        }

        if (Input.GetKey(KeyCode.W))
        {
            desiredMovement += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            desiredMovement += Vector3.back;
        }

        _agent.Move(_agent.speed * desiredMovement * Time.deltaTime);
    }
}
