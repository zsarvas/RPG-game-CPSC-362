using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float walkSpeed;
    public float runSpeed;
    public Animator animator;
    public Rigidbody2D rb;
    public string runButton;

    private Vector2 movement;
    private bool xflag;
    private bool yflag;
    private bool running;
    private float movHor;
    private float movVer;
    private float speed;


    // Update is called on a fixed interval
    void FixedUpdate()
    {   
        //version1.5
        movHor = Input.GetAxisRaw("Horizontal");
        movVer = Input.GetAxisRaw("Vertical");
        //disables diagonal movement
        if (movHor != 0 && movVer != 0) {//is it diagonal?
            if (xflag) { //ignore x input if x was first
                movHor = 0;
            }
            else { //ignore y if y was first
                movVer = 0;
            }
        }
        else { //only one direction?
            xflag = movHor != 0; //check if direction is x
            yflag = movVer != 0; //check if direction is y
        }


        //Increase player speed if x is held down
        if (Input.GetKey(runButton.ToLower())) {
            speed = runSpeed;
            running = true;
        }
        else {
            speed = walkSpeed;
            running = false;
        }


        movement = new Vector2(movHor, movVer);

        //set float values for animator to decide sprite animation
        animator.SetFloat("Horizontal", movHor);
        animator.SetFloat("Vertical", movVer);
        if (movement.sqrMagnitude != 0f) {
            //Debug.Log("In mag != 0");
            if (running) {
                //Debug.Log("Running");
                animator.SetFloat("Speed", 1f);
            }
            else {
                //Debug.Log("Walking");
                animator.SetFloat("Speed", 0.5f);
            }
        }
        else {
            //Debug.Log("In mag == 0");
            animator.SetFloat("Speed", 0f);
        }
       
        //animator.SetFloat("Speed", movement.sqrtMagnitude);
        //Debug.Log(movement.sqrMagnitude);

        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
