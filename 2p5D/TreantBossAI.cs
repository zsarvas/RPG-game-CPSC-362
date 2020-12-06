using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantBossAI : MonoBehaviour
{
    public GameObject[] walls;
    public GameObject spikeTree;
    public GameObject treant;
    public Transform[] spawnPos;
    public Transform checkMobs;
    public Animator animator;
    public Vector3 fallSpeed = new Vector3(0, -100, 0);
    public float wallSpawnSpeed = 2f;
    public float _lastDirection;

    private GameObject guardWall;
    private Rigidbody rb;
    private EnemyCombat combatScript;
    private Vector3 movement;
    private bool moving;
    private bool ready;
    private bool meleePhase;
    private bool wallPhase;
    private int currPhase;
    private int currWall;
    private int rageWalls;
    private int lastHealth;
    private float movHor;
    private float movVer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        combatScript = GetComponent<EnemyCombat>();
        GetComponent<SpriteRenderer>().enabled = false;
        lastHealth = combatScript.currHP;
        currPhase = 0;
        PhaseStart();
    }

    void Update()
    {
        DeathCheck();

        if (rageWalls == 2)
        {
            PhaseStart();
        }
    }

    void DeathCheck()
    {
        if (combatScript.currHP > 0) return;

        this.enabled = false;
    }

    private IEnumerator WallPhase(float interval, float wallSpeed)
    {
        while (lastHealth - combatScript.currHP != 1)
        {
            yield return new WaitForSeconds(interval);
            currWall = Random.Range(0, 4);
            GameObject wall = Instantiate(walls[currWall], spawnPos[currWall].position, spawnPos[currWall].rotation, transform);
            if (wall.transform.childCount > 1)
            {
                foreach (BossWall w in wall.GetComponentsInChildren<BossWall>())
                {
                    w.speed = wallSpeed;
                }
            }
            else
            {
                wall.GetComponent<BossWall>().speed = wallSpeed;
            }
        }

        lastHealth = combatScript.currHP;
        //todo: dont rage wall after death
        StartCoroutine("RageWall");
    }

    private IEnumerator RageWall()
    {
        Debug.Log("Rage Wall begin");
        GameObject useWall = guardWall != null ? guardWall : Instantiate(walls[4], spawnPos[4].position, spawnPos[4].rotation, transform);
        useWall.GetComponent<BossWall>().enabled = true;

        while (useWall != null)
        {
            yield return null;
        }

        Debug.Log("Rage Wall end");
        rageWalls += 1;
    }

    private IEnumerator StartMeleePhase(int treantCount)
    {
        Debug.Log("Melee phase begin");

        for (int i = 6; i < treantCount + 6; i++)
        {
            GameObject t = Instantiate(treant, spawnPos[i].position, spawnPos[i].rotation, checkMobs);
            //t.GetComponent<EnemyCombat>().immuneTo = new float[] {0f};
        }

        while (checkMobs.childCount != 0)
        {
            yield return null;
        }

        //power up ranged phase if it's the mixed phase
        bool b = currPhase == 3 ? true : false;

        PhaseChange(b);
    }

    void StartRangedPhase(float aimSpeed)
    {
        Debug.Log("ranged phase begin");
        GameObject sTreeObj = Instantiate(spikeTree, spawnPos[5].position, spawnPos[5].rotation, transform);
        sTreeObj.GetComponent<BossSpikeTreeAI>().aimTime = aimSpeed;
    }

    public void PhaseChange(bool upRanged = false)
    {
        switch (currPhase)
        {
            //opening -> melee
            case 0:
                //do nothing
                break;
            //melee -> ranged
            case 1:
                Debug.Log("melee phase end");
                StartCoroutine("RageWall");
                StartCoroutine(WallPhase(1.25f, 8f));
                break;
            //ranged -> mixed
            case 2:
                Debug.Log("ranged phase end");
                StartCoroutine("RageWall");
                StartCoroutine(WallPhase(1f, 10f));
                break;
            //mixed -> halfway
            case 3:
                Debug.Log("halfway through mixed phase");
                if (upRanged)
                {
                    transform.Find("BossSpikeTree(Clone)").GetComponent<BossSpikeTreeAI>().aimTime = 0f;
                }
                else
                {
                    //do something for melee
                }
                break;
            //halfway -> final wall phase
            case 4:
                Debug.Log("final wall phase");
                StartCoroutine("RageWall");
                StartCoroutine(WallPhase(0.75f, 12f));
                //StartCoroutine(WallPhase(0.5f, 14f)); //very hard but possible
                break;
            default:
                Debug.Log("error: " + name + " PhaseChange() " + currPhase);
                Destroy(this.gameObject);
                break;
        }
        currPhase += 1;
    }

    void PhaseStart()
    {
        rageWalls = 0;

        switch (currPhase)
        {
            //begin
            case 0:
                currPhase += 1;
                PhaseStart();
                break;
            //melee
            case 1:
                guardWall = Instantiate(walls[4], spawnPos[4].position, spawnPos[4].rotation, transform);
                StartCoroutine(StartMeleePhase(3));
                break;
            //ranged
            case 2:
                guardWall = Instantiate(walls[4], spawnPos[4].position, spawnPos[4].rotation, transform);
                StartRangedPhase(5f);
                break;
            //mixed
            case 3:
                guardWall = Instantiate(walls[4], spawnPos[4].position, spawnPos[4].rotation, transform);
                StartRangedPhase(1f);
                StartCoroutine(StartMeleePhase(5));
                break;
            //halfway 
            case 4:
                //do nothing
                break;
            //Death Sequence
            case 5:
                //todo: slower version of normal death?
                //todo: make it drop TreeGem
                Debug.Log("Congratulations!");
                break;
            default:
                Debug.Log("error: " + name + " PhaseStart() " + currPhase);
                Destroy(this.gameObject);
                break;
        }
    }

    private IEnumerator Wait(float t)
    {
        yield return new WaitForSeconds(t);
        PhaseChange();
    }
}
