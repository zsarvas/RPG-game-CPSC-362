using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rb;
    public Vector3 fallSpeed = new Vector3(0, -100, 0);
    public float speed = 1f;

    private Vector3 movement;
    private float movHor;
    private float movVer;
    private Vector3 PlayerDirection;
    private bool moving;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        speed = 1;
        ResetRoutine();
        if (!moving) {
            StartCoroutine("Walk");
        }
        movement = new Vector3(movHor, 0, movVer);
        SetAnimations();
        StickToGround();
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    void ResetRoutine() {
        if (Input.GetKeyDown("p")) {
            StopCoroutine("Walk");
            moving = false;
        }
    }

    void StickToGround() {
        //stick to ground if moving
        if (movement.sqrMagnitude != 0f)
        {
            rb.velocity = fallSpeed;
        }
        else
        {
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

    private IEnumerator Walk() {
        moving = true;
        //move right for 1s then idle for 1s
        Move(1f, 0f);
        yield return new WaitForSeconds(1f);
        Idle();
        yield return new WaitForSeconds(1f);
        //move up for 1s then idle for 1s
        Move(0f, 1f);
        yield return new WaitForSeconds(1f);
        Idle();
        yield return new WaitForSeconds(1f);
        //move left for 1s then idle for 1s
        Move(-1f, 0f);
        yield return new WaitForSeconds(1f);
        Idle();
        yield return new WaitForSeconds(1f);
        //move down for 1s then idle for 1s
        Move(0f, -1f);
        yield return new WaitForSeconds(1f);
        Idle();
        yield return new WaitForSeconds(1f);
        moving = false;
    }

    void Idle() {
        movHor = 0f;
        movVer = 0f;
    }

    void Move(float x, float z) {
        movHor = x;
        movVer = z;
    }

    void SetAnimations()
    {
        //set bool value for transition condition between walking and idle in animator
        animator.SetFloat("Horizontal", movHor);
        animator.SetFloat("Vertical", movVer);
        if (movement.sqrMagnitude != 0f)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
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
    }
}
