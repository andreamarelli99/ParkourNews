using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerController controller;
    public Animator animator;
    private float _horizontalMove = 0f;
    public float runSpeed = 40;

    private bool _jump;
    private bool _slide;
    private bool _run;
    void Start()
    {
        _jump = false;
        _slide = false;
        _run = false;
    }

    void Update()
    {
        _horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed",Mathf.Abs(_horizontalMove));
        
        if (Input.GetButtonDown("Jump"))
        {
            _jump = true;
            animator.SetBool("IsJumping",true);
        }
        
        if (Input.GetButtonDown("Fire3"))
        {
            _run = true;
            runSpeed *= 2;
            animator.SetBool("IsRunning",true);
        }else if (Input.GetButtonUp("Fire3"))
        {
            runSpeed /= 2;
            animator.SetBool("IsRunning",false);
        }

        if (Input.GetButtonDown("Slide"))
        {
            _slide = true;
        } else if (Input.GetButtonUp("Slide"))
        {
            _slide = false;
        }
        
        
    }

    public void onLanding()
    {
        _jump = false;
        animator.SetBool("IsJumping",false);
    }

    private void FixedUpdate()
    {
        controller.Move(_horizontalMove * Time.fixedDeltaTime,false,_jump);
        _jump = false;
        //move
    }
}
