using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCharMovementController : DamageReceiver, ICanHeal
{
    private const int MAX_LIFE = 16;

    private Animator animator;
    private CharacterController controller;

    public float speed = 5.0f;
    public float strafeSpeed = 3.0f;
    public float gravity = 10f;
    public float jumpSpeed = 10.0f;
    private Vector3 fallVec = Vector3.zero;
    public bool IsJumping { get; set; } = false;

    public DamageCollider swordCollider;
    public DamageColorChanger dmgColorChanger;

    public int life = MAX_LIFE;
    public HearthsDisplay lifeDisplay;
    public bool IsDead { get; set; }

    public AudioSource audioFootSteps;
    public AudioSource audioJumpEnd;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        animator.GetBehaviour<SwordHitAnimationBehaviour>().swordCollider = swordCollider;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

            if (controller.isGrounded) audioFootSteps.volume = move.magnitude * 0.5f;
            else audioFootSteps.volume = 0;

            animator.SetFloat("forwardSpeed", Input.GetAxis("Vertical"));
            animator.SetFloat("sidestepSpeed", Input.GetAxis("Horizontal"));
            if (!controller.isGrounded)
            {
                fallVec.y -= gravity * Time.deltaTime;
                controller.Move(fallVec * Time.deltaTime);
            }
            else
            {
                fallVec.y = 0;
                if (IsJumping)
                {
                    audioJumpEnd.Play();
                    animator.SetTrigger("jumpEnd");
                    IsJumping = false;
                }
                if (Input.GetAxis("Jump") > 0.5)
                {
                    animator.SetTrigger("jumpStart");
                    fallVec.y = jumpSpeed;
                    controller.Move(fallVec * Time.deltaTime);
                    IsJumping = true;
                }
            }

            if (Input.GetButtonDown("Fire1"))
            {
                animator.SetTrigger("swordHit");
            }
        }
        else if (Input.GetKey(KeyCode.Return))
        {
            //Application.LoadLevel(Application.loadedLevel);
            CoinScript.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
    }

    public override bool IsPlayer()
    {
        return true;
    }

    public void Die()
    {
        if (!IsDead)
        {
            GameObject.FindObjectOfType<MusicManager>().PlayDeadMusic();
            IsDead = true;
            animator.SetTrigger("die");
            lifeDisplay.Die();
        }
    }

    public override void DoDamage()
    {
        Debug.Log("MainChar DoDamage()");
        if (life > 0) dmgColorChanger.ShowDamage();
        life -= 1;
        if (life <= 0) Die();
        lifeDisplay.SetValue(life);
    }

    public void RecoverFullHealth()
    {
        life = MAX_LIFE;
        lifeDisplay.SetValue(life);
    }
}
