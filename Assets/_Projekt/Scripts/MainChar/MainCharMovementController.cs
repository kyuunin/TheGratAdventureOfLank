using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharMovementController : MonoBehaviour
{
    private Animator animator;
    private CharacterController controller;

    public float speed = 5.0f;
    public float strafeSpeed = 3.0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    
    }
    
    void Update()
    {
        var move = new Vector3(0, 0, 0);
        move -= transform.forward * speed * Input.GetAxis("Vertical");
        move -= transform.right * strafeSpeed * Input.GetAxis("Horizontal");

        controller.SimpleMove(move);
    }
}
