using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCharMovementController : DamageReciever
{
    private Animator animator;
    private CharacterController controller;

    public float speed = 5.0f;
    public float strafeSpeed = 3.0f;

    public DamageCollider swordCollider;

    public int life = 8;
    public HearthsDisplay lifeDisplay;
    public bool IsDead { get; set; }

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        animator.GetBehaviour<SwordHitAnimationBehaviour>().swordCollider = swordCollider;

        Cursor.lockState = CursorLockMode.Locked;
        IsDead = false;
        lifeDisplay.SetValue(life);
    }
    
    void Update()
    {
        if (!IsDead)
        {
            var move = new Vector3(0, 0, 0);
            move -= transform.forward * speed * Input.GetAxis("Vertical");
            move -= transform.right * strafeSpeed * Input.GetAxis("Horizontal");

            controller.SimpleMove(move);
            animator.SetFloat("forwardSpeed", Input.GetAxis("Vertical"));
            animator.SetFloat("sidestepSpeed", Input.GetAxis("Horizontal"));

            if (Input.GetButtonDown("Fire1"))
            {
                animator.SetTrigger("swordHit");
            }
        }
        else if (Input.GetKey(KeyCode.Return)) {
            //Application.LoadLevel(Application.loadedLevel);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            CoinScript.Reset();
            
        }
    }

    public override bool IsPlayer()
    {
        return true;
    }

    public void Die() {
        if (!IsDead)
        {
            IsDead = true;
            animator.SetTrigger("die");
            lifeDisplay.Die();
        }
    }

    public override void DoDamage()
    {
        Debug.Log("MainChar DoDamage()");
        life -= 1;
        if (life <= 0) Die();
        lifeDisplay.SetValue(life);
    }
}
