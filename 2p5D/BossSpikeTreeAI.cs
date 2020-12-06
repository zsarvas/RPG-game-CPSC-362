using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpikeTreeAI : MonoBehaviour
{
    public GameObject treeSpike;
    public GameObject targetReticle;
    public GameObject spikeGuard;
    public Transform[] guardPos;
    public float aimTime = 5f;
    public bool lockedOn = false;

    private Vector3 offset;
    private GameObject player;
    private Transform playerT;
    private Transform target;
    private GameObject targetInstance;
    private EnemyCombat combatScript;
    private Animator ani;
    private bool done;
    private int lastHP;
    private int animationState;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, -0.736432f, 0);
        player = GameObject.Find("PlayerQuad");
        playerT = player.GetComponent<Transform>();
        combatScript = GetComponent<EnemyCombat>();
        lastHP = combatScript.currHP;
        ani = transform.Find("renderQuad").GetComponent<Animator>();
        done = false;
        animationState = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (combatScript.currHP == 0 && !done)
        {
            ChangeState();
            done = true;
            transform.parent.gameObject.GetComponent<TreantBossAI>().PhaseChange();
        }
        else if (combatScript.currHP > 0 && lastHP - combatScript.currHP == 1)
        {
            ChangeState();
            foreach (Transform pos in guardPos)
            {
                Instantiate(spikeGuard, pos.position, pos.rotation, transform);
            }
            lastHP = combatScript.currHP;
        }

        if (!lockedOn)
        {
            targetInstance = Instantiate(targetReticle, playerT.position + offset, Quaternion.identity, transform);
            target = targetInstance.GetComponent<Transform>();
            StartCoroutine("Aim");
        }
    }

    private IEnumerator Aim()
    {
        lockedOn = true;
        float t = 0f;

        while (t <= aimTime)
        {
            t += Time.deltaTime;
            target.position = playerT.position + offset;
            yield return null;
        }

        Destroy(target.gameObject);
        Instantiate(treeSpike, target.position - offset, playerT.rotation, transform);
    }

    void ChangeState()
    {
        animationState += 1;
        ani.SetInteger("State", animationState);
    }
}
