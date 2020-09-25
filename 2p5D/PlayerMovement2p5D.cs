using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2p5D : MonoBehaviour
{

    public float walkSpeed;
    public float runSpeed;
    public Animator animator;
    public Rigidbody rb;

    private Vector3 movement;
    private bool xflag;
    private bool yflag;
    private bool running;
    private float movHor;
    private float movVer;
    private float speed;
    private Vector3 PlayerDirection;


    // Update is called on a fixed interval
    void FixedUpdate()
    {
        //version1.5
        movHor = Input.GetAxisRaw("Horizontal");
        movVer = Input.GetAxisRaw("Vertical");

        //disables diagonal movement
        if (movHor != 0 && movVer != 0)
        {//is it diagonal?
            if (xflag)
            { //ignore x input if x was first
                movHor = 0;
            }
            else
            { //ignore y if y was first
                movVer = 0;
            }
        }
        else
        { //only one direction?
            xflag = movHor != 0; //check if direction is x
            yflag = movVer != 0; //check if direction is y
        }

        //Increase player speed if x is held down
        if (Input.GetButton("Cancel"))
        {
            speed = runSpeed;
            running = true;
        }
        else
        {
            speed = walkSpeed;
            running = false;
        }

        movement = new Vector3(movHor, 0, movVer);

        //set float values for animator to decide sprite animation
        animator.SetFloat("Horizontal", movHor);
        animator.SetFloat("Vertical", movVer);
        if (movement.sqrMagnitude != 0f)
        {
            if (running)
            {
                animator.SetFloat("Speed", 1f);
            }
            else
            {
                animator.SetFloat("Speed", 0.5f);
            }
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }

        //create collisions using raycast to prevent player from running through collision boxes
        RaycastHit TheHit;
        switch (movHor) {
            case -1: PlayerDirection = Vector3.left;
                break;
            case 1: PlayerDirection = Vector3.right;
                break;
            default: break;
        }
        switch (movVer) {
            case -1: PlayerDirection = Vector3.back;
                break;
            case 1: PlayerDirection = Vector3.forward;
                break;
            default: break;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(PlayerDirection), out TheHit, 0.25f))
        {
            speed = 0;
        }

        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
