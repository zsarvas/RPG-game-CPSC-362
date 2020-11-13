using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float walkSpeed = 6f;
    public float runSpeed = 10f;
    public Animator animator;
    public Rigidbody rb;
    public string runButton = "z";
    public Vector3 fallSpeed = new Vector3(0, -100, 0);
    public float _lastDirection;

    private Vector3 movement;
    private bool xflag;
    private bool yflag;
    private float movHor;
    private float movVer;
    private float speed;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        GetComponent<SpriteRenderer>().enabled = false;
    }


    void Update()
    {
        movHor = Input.GetAxisRaw("Horizontal");
        movVer = Input.GetAxisRaw("Vertical");
        DisableDiagonal();
        movement = new Vector3(movHor, 0, movVer);
        animator.SetFloat("Horizontal", movHor);
        animator.SetFloat("Vertical", movVer);
        if (movement.magnitude == 0)
        {
            animator.SetBool("Idle", true);
        }
        else
        {
            animator.SetBool("Idle", false);
            _lastDirection = GetLastDirection();
            animator.SetFloat("LastDirection", _lastDirection);
        }
        speed = Run();
    }


    // Update is called on a fixed interval
    void FixedUpdate()
    {
        StickToGround();
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    //Increase player speed if run button is held down
    float Run()
    {
        if (Input.GetKey(runButton.ToLower()))
        {
            animator.SetBool("Running", true);
            return runSpeed;
        }
        animator.SetBool("Running", false);
        return walkSpeed;
    }

    //disables diagonal movement
    void DisableDiagonal()
    {
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
    }

    //gets last direction player was facing
    float GetLastDirection()
    {
        //0 = up, 1 = right, 2 = down, 3 = left
        switch (movHor)
        {
            case 0:
                break;
            case -1:
                return 3f;
            case 1:
                return 1f;
            default: break;
        }
        switch (movVer)
        {
            case -1:
                return 2f;
            case 1:
                return 0f;
            default: break;
        }
        Debug.Log("Error: Getting last direction while not moving");
        return -1;
    }

    //enables falling if moving and prevents sliding if still
    void StickToGround()
    {
        if (movement.sqrMagnitude != 0)
        {
            rb.velocity = fallSpeed;
            return;
        }
        rb.velocity = Vector3.zero;
    }
}
