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

    private bool frozen;
    private Transform currP;
    private float lastDir;
    private float currDir;

    // Start is called before the first frame update
    void Start()
    {
        atkPUp = transform.Find("attackPointU").GetComponent<Transform>();
        atkPRight = transform.Find("attackPointR").GetComponent<Transform>();
        atkPDown = transform.Find("attackPointD").GetComponent<Transform>();
        atkPLeft = transform.Find("attackPointL").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        currDir = 1f;
        lastDir = currDir;
    }

    // Update is called once per frame
    void Update()
    {
        if (frozen) return;
        if (Input.GetKeyDown(SwordAtkButton.ToLower()))
        {
            if (meleeOn)
            {
                Melee_Attack(1f);
            }
        }
        else if (Input.GetKeyDown(BowAtkButton.ToLower()))
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
        GameObject.Find("SwordSwing").GetComponent<AudioSource>().Play();
        currP = UpdateAtkPoint();
        Collider[] hitEnemies = Physics.OverlapSphere(currP.position, attackRange, enemyLayers);
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyCombat>().DamageEnemy(damage);
        }
    }

    void Ranged_Attack(float type)
    {
        Start_Attack(type);
        GameObject.Find("ArrowShoot").GetComponent<AudioSource>().Play();
        currP = UpdateAtkPoint();
        GameObject arrow = Instantiate(arrowPrefab, currP.position, currP.rotation);
        Rigidbody arrowRb = arrow.GetComponent<Rigidbody>();
        Transform arrowT = arrow.GetComponent<Transform>();
        switch (GetComponent<PlayerMovement>()._lastDirection)
        {
            case 0:
                arrowT.Rotate(90f, 0f, 0f);
                arrowRb.AddForce(Vector3.forward * arrowForce, ForceMode.Impulse);
                break;
            case 1:
                arrowT.Rotate(0f, 0f, -90f);
                arrowRb.AddForce(Vector3.right * arrowForce, ForceMode.Impulse);
                break;
            case 2:
                arrowT.Rotate(90f, 0f, 180f);
                arrowRb.AddForce(Vector3.back * arrowForce, ForceMode.Impulse);
                break;
            case 3:
                arrowT.Rotate(0f, 0f, 90f);
                arrowRb.AddForce(Vector3.left * arrowForce, ForceMode.Impulse);
                break;
            default:
                Debug.Log("Error: ranged direction");
                break;
        }
    }

    private IEnumerator Freeze(float time) {
        frozen = true;
        GetComponent<PlayerMovement>().enabled = false;
        yield return new WaitForSeconds(time);
        GetComponent<PlayerMovement>().enabled = true;
        frozen = false;
    }
    
    Transform UpdateAtkPoint()
    {
        currDir = GetComponent<PlayerMovement>()._lastDirection;
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
    
}
