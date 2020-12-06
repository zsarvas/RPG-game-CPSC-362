using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rb;
    public Vector3 fallSpeed = new Vector3(0, -100, 0);
    public float speed = 1f;
    public float _lastDirection;

    private Vector3 movement;
    private float movHor;
    private float movVer;
    private bool moving;
    private EnemyCombat combatScript;
    private EnemyContactDamage damageScript;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        combatScript = GetComponent<EnemyCombat>();
        damageScript = GetComponent<EnemyContactDamage>();
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update()
    {
        DeathCheck();
        ResetRoutine();

        if (!moving) StartCoroutine("Walk");

        movement = new Vector3(movHor, 0, movVer);
        animator.SetFloat("Horizontal", movHor);
        animator.SetFloat("Vertical", movVer);

        //Debug.DrawRay(transform.position, Vector3.forward * 5f, Color.red);

        if (movement.sqrMagnitude == 0f)
        {
            animator.SetBool("Moving", false);
        }
        else
        {
            _lastDirection = GetLastDirection();
            animator.SetFloat("LastDirection", _lastDirection);
            animator.SetBool("Moving", true);
        }

        damageScript.lastDir = GetDirVector();
    }

    void FixedUpdate()
    {
        StickToGround();
        transform.Translate(movement * speed * Time.fixedDeltaTime);
    }

    //reset walking routine when reseting position
    void ResetRoutine() {
        if (Input.GetKeyDown("p")) {
            StopCoroutine("Walk");
            moving = false;
        }
    }

    //enables falling if moving and prevents sliding if still
    void StickToGround() {
        if (movement.sqrMagnitude != 0f)
        {
            rb.velocity = fallSpeed;
            return;
        }
        rb.velocity = Vector3.zero;
    }

    //walk in a box
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

    //gets last direction enemy was facing
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

    void DeathCheck()
    {
        if (combatScript.currHP > 0) return;

        this.enabled = false;
    }

    Vector3 GetDirVector()
    {
        switch (_lastDirection)
        {
            case 0:
                return transform.forward;
            case 1:
                return transform.right;
            case 2:
                return -transform.forward;
            case 3:
                return -transform.right;
            default:
                Debug.Log("error:" + name + " : GetDirVector()");
                return transform.forward;
        }
    }
}
