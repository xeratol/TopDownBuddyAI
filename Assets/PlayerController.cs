using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _agent;

    void Start()
    {
        Debug.Assert(_agent, "Agent not set", this);
    }

    void Update()
    {
        //if (Input.GetMouseButton(0))
        //{
        //    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    var plane = new Plane(Vector3.up, Vector3.zero);
        //    var t = 0.0f;
        //    if (plane.Raycast(ray, out t))
        //    {
        //        var point = ray.GetPoint(t);
        //        _agent.SetDestination(point);
        //    }
        //}

        UpdatePosition();

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
            desiredMovement += _agent.speed * Vector3.left * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            desiredMovement += _agent.speed * Vector3.right * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W))
        {
            desiredMovement += _agent.speed * Vector3.forward * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            desiredMovement += _agent.speed * Vector3.back * Time.deltaTime;
        }

        _agent.Move(desiredMovement);
    }
}
