using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobiMovement : MonoBehaviour
{
    public float maxDistance = 20.0f;
    public float jumpHeight = 1.0f;
    public float jumpDistance = 1.0f;
    public float jumpTimeout = 3.0f;

    private Rigidbody rigidb;
    private Transform target;

    private float jumpTimer;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rigidb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        var direction = target.position - transform.position;
        var xzDirection = new Vector3(direction.x, 0, direction.z).normalized;

        
        if (direction.magnitude < maxDistance) {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer < 0)
            {
                rigidb.AddForce(xzDirection * jumpDistance + Vector3.up * jumpHeight);
                jumpTimer = jumpTimeout;
            }
        }

        rigidb.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(-90, 0, 0);
    }
}
