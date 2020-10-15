using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float walkSpeed;
    public float runSpeed;
    public Animator animator;
    public Rigidbody rb;
    public string runButton = "z";
    public string BowAtkButton = "x";
    public string SwordAtkButton = "c";
    public Vector3 fallSpeed = new Vector3(0, -100, 0);

    private Vector3 movement;
    private bool xflag;
    private bool yflag;
    private bool running;
    private float movHor;
    private float movVer;
    private float speed;
    private bool frozen;
    private Vector3 PlayerDirection;


    void Start() {
        GetComponent<SpriteRenderer>().enabled = false;
    }


    void Update() {
        if (!frozen)
        {
            SetAnimations();
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }

    }


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
        if (Input.GetKey(runButton.ToLower()))
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

        //make player stick to ground if moving
        if (movement.sqrMagnitude != 0f)
        {
            rb.velocity = fallSpeed;
        }
        else
        {
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

    private IEnumerator Freeze() {
        frozen = true;
        yield return new WaitForSeconds(.4f);
        frozen = false;
    }

    void SetAnimations() {
        //set float values for animator to decide sprite animation during movement
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
        //also sets last direction for player animator to set idle animation
        RaycastHit TheHit;
        switch (movHor)
        {
            case -1:
                PlayerDirection = Vector3.left;
                animator.SetFloat("LastDirection", 3);
                break;
            case 1:
                PlayerDirection = Vector3.right;
                animator.SetFloat("LastDirection", 1);
                break;
            default: break;
        }
        switch (movVer)
        {
            case -1:
                PlayerDirection = Vector3.back;
                animator.SetFloat("LastDirection", 2);
                break;
            case 1:
                PlayerDirection = Vector3.forward;
                animator.SetFloat("LastDirection", 0);
                break;
            default: break;
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(PlayerDirection), out TheHit, 0.25f))
        {
            speed = 0;
        }

        //set animations for attacking
        if (Input.GetKeyDown(BowAtkButton.ToLower()))
        {
            animator.SetFloat("AtkType", 0f);
            animator.SetTrigger("attack");
            StartCoroutine(Freeze());
        }
        else if (Input.GetKeyDown(SwordAtkButton.ToLower())) {
            animator.SetFloat("AtkType", 1f);
            animator.SetTrigger("attack");
            StartCoroutine(Freeze());
        }
    }
}
