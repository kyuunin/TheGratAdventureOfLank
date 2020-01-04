using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharMovementController : DamageReciever
{
    private Animator animator;
    private CharacterController controller;

    public float speed = 5.0f;
    public float strafeSpeed = 3.0f;

    public DamageCollider swordCollider;

    public int life = 5;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        animator.GetBehaviour<SwordHitAnimationBehaviour>().swordCollider = swordCollider;

        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        var move = new Vector3(0, 0, 0);
        move -= transform.forward * speed * Input.GetAxis("Vertical");
        move -= transform.right * strafeSpeed * Input.GetAxis("Horizontal");

        controller.SimpleMove(move);
        animator.SetFloat("forwardSpeed", Input.GetAxis("Vertical"));
        animator.SetFloat("sidestepSpeed", Input.GetAxis("Horizontal"));

        if(Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("swordHit");
        }
    }

    public override bool IsPlayer()
    {
        return true;
    }

    public override void DoDamage()
    {
        Debug.Log("MainChar DoDamage()");
        life -= 1;
    }
}
