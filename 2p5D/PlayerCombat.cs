using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform atkPUp;
    public Transform atkPRight;
    public Transform atkPDown;
    public Transform atkPLeft;
    public Animator animator;
    public LayerMask enemyLayers;
    public GameObject arrowPrefab;
    public string SwordAtkButton = "c";
    public string BowAtkButton = "x";
    public bool meleeOn = false;
    public bool rangeOn = true;
    public int damage = 1;
    public float attackRange = 0.5f;
    public float arrowForce = 20f;
    public float meleeForce = 5f;

    private Transform currP;
    private AudioSource arrowShoot;
    private AudioSource swordSwing;
    private PlayerMovement movScript;
    private PlayerCasting castScript;
    private bool frozen;
    private float lastDir;
    private float currDir;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movScript = GetComponent<PlayerMovement>();
        castScript = GetComponent<PlayerCasting>();
        atkPUp = transform.Find("attackPointU").GetComponent<Transform>();
        atkPRight = transform.Find("attackPointR").GetComponent<Transform>();
        atkPDown = transform.Find("attackPointD").GetComponent<Transform>();
        atkPLeft = transform.Find("attackPointL").GetComponent<Transform>();
        swordSwing = GameObject.Find("SwordSwing").GetComponent<AudioSource>();
        arrowShoot = GameObject.Find("ArrowShoot").GetComponent<AudioSource>();
        currDir = 1f;
        lastDir = currDir;
    }

    // Update is called once per frame
    void Update()
    {
        if (frozen) return;

        if (Input.GetKeyDown(SwordAtkButton.ToLower()) || Input.GetMouseButtonDown(0))
        {
            if (meleeOn)
            {
                Melee_Attack(1f);
            }
        }
        else if (Input.GetKeyDown(BowAtkButton.ToLower()) || Input.GetMouseButtonDown(1))
        {
            if (rangeOn)
            {
                Ranged_Attack(0f);
            }
        }
    }

    void Start_Attack(float type)
    {
        StartCoroutine(Freeze(0.4f));
        animator.SetFloat("AtkType", type);
        animator.SetTrigger("attack");
    }

    void Melee_Attack(float type) {
        Start_Attack(type);
        swordSwing.Play();
        currP = UpdateAtkPoint();
        Collider[] hitEnemies = Physics.OverlapSphere(currP.position, attackRange, enemyLayers);
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyCombat>().DamageEnemy(damage, meleeForce, GetKnockDir(), type);
        }
    }

    void Ranged_Attack(float type)
    {
        Start_Attack(type);
        arrowShoot.Play();
        currP = UpdateAtkPoint();
        GameObject arrow = Instantiate(arrowPrefab, currP.position, arrowPrefab.transform.rotation);
        Rigidbody arrowRb = arrow.GetComponent<Rigidbody>();
        Transform arrowT = arrow.GetComponent<Transform>();
        arrowT.rotation = transform.rotation;
        //Debug.DrawRay(transform.position, arrowT.up * 20f, Color.yellow);
        switch (movScript._lastDirection)
        {
            case 0:
                //arrowT.rotation = Quaternion.LookRotation(transform.forward, transform.forward);
                arrowT.Rotate(90f, 0f, 0f);
                arrowRb.AddForce(transform.forward * arrowForce, ForceMode.Impulse);
                break;
            case 1:
                arrowT.Rotate(90f, 90f, 0f);
                arrowRb.AddForce(transform.right * arrowForce, ForceMode.Impulse);
                break;
            case 2:
                arrowT.Rotate(90f, 180f, 0f);
                arrowRb.AddForce(-transform.forward * arrowForce, ForceMode.Impulse);
                break;
            case 3:
                arrowT.Rotate(90f, -90f, 0f);
                arrowRb.AddForce(-transform.right * arrowForce, ForceMode.Impulse);
                break;
            default:
                Debug.Log("Error: ranged direction");
                break;
        }
    }

    private IEnumerator Freeze(float time) {
        frozen = true;
        movScript.enabled = false;
        yield return new WaitForSeconds(time);
        movScript.enabled = true;
        frozen = false;
    }
    
    Transform UpdateAtkPoint()
    {
        currDir = movScript._lastDirection;
        switch (currDir)
        {
            case 0:
                return atkPUp;
            case 1:
                return atkPRight;
            case 2:
                return atkPDown;
            case 3:
                return atkPLeft;
            default:
                Debug.Log("Error: Update Attack Point returned null");
                return null;
        }
    }

    public Vector3 GetKnockDir()
    {
        switch (movScript._lastDirection)
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
                Debug.Log("error: " + name + "GetKnockDir()");
                return transform.forward;
        }
    }
}
