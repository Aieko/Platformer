using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement2D : MonoBehaviour
{
    public CharacterController2D controller;

    public Animator animator;

    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    private void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("MeleeAttack"))
           {

            controller.Move(0, false, false);
 
            return;
            
           }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            if(controller.m_FacingRight)
            controller.Move(0.03f, false, false);
            else
                controller.Move(-0.03f, false, false);

            return;
        }
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
      
    }
}
