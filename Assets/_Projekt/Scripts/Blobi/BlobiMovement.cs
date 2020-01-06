using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobiMovement : DamageReceiver
{
    public AudioSource audioHit;

    public float maxDistance = 20.0f;
    public float jumpHeight = 1.0f;
    public float jumpDistance = 1.0f;
    public float jumpTimeout = 3.0f;

    public float knockbackJumpFactor = 1.0f;
    public float knockbackMoveFactor = 1.0f;

    public int life = 3;
    public bool gameWonAfterKill = false;

    private Rigidbody rigidb;
    private Transform target;

    private float jumpTimer;

    public override void DoDamage()
    {
        Debug.Log("Blobi DoDamage()");
        audioHit.Play();
        life -= 1;
        if (life == 0)
        {
            audioHit.transform.parent = transform.parent; // Detach audio source to not destroy it, we want to hear the sound
            Destroy(audioHit.gameObject, 2.0f); // clean up sound after use

            if (gameWonAfterKill)
                GameObject.FindGameObjectWithTag("Player").GetComponent<WinScreenDisplay>().Win();
            Destroy(gameObject);
        }
        else
        {
            GetComponent<DamageColorChanger>().ShowDamage();

            var direction = target.position - transform.position;
            var xzDirection = new Vector3(direction.x, 0, direction.z).normalized;
            rigidb.AddForce(-xzDirection * jumpDistance * knockbackMoveFactor + Vector3.up * jumpHeight * knockbackJumpFactor);
        }
    }

    public override bool IsPlayer()
    {
        return false;
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rigidb = GetComponent<Rigidbody>();
    }
    
    private void Jump(Vector3 direction)
    {
        rigidb.AddForce(direction * jumpDistance + Vector3.up * jumpHeight);
    }

    void Update()
    {
        var direction = target.position - transform.position;
        var xzDirection = new Vector3(direction.x, 0, direction.z).normalized;

        
        if (direction.magnitude < maxDistance) {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer < 0)
            {
                Jump(xzDirection);
                jumpTimer = jumpTimeout;
            }
        }

        rigidb.rotation = Quaternion.LookRotation(xzDirection) * Quaternion.Euler(-90, 0, 0);
    }
}
