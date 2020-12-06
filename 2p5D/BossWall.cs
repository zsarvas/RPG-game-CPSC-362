using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWall : MonoBehaviour
{
    public bool isMulti;
    public bool rage;
    public float speed;
    public string playSFX;
    public string loopSFX;

    private GameObject player;
    private GameObject sfx;
    private GameObject loop;
    private Transform playerT;
    private PlayerMovement movScript;
    private PlayerCombat combatScript;
    private EnemyCombat myCombat;
    private float playerYStart;

    void Start()
    {
        if (playSFX != "")
        {
            sfx = GameObject.Find(playSFX);
            sfx = Instantiate(sfx, transform);
            sfx.GetComponent<AudioSource>().Play();
        }

        if (loopSFX != "")
        {
            loop = GameObject.Find(loopSFX);
            loop = Instantiate(loop, transform.position, transform.rotation,  transform);
            if (rage)
            {
                loop.GetComponent<AudioSource>().volume = 0.7f;
            }
            loop.GetComponent<AudioSource>().Play();
        }

        myCombat = GetComponent<EnemyCombat>();
        //StartCoroutine("Kill");
    }

    void Update()
    {
        if (rage) return;

        if (myCombat.currHP <= 0)
        {
            freePlayer();
        }
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.back * speed * Time.fixedDeltaTime);

        if (player != null)
        {
            playerT.position = new Vector3(transform.position.x+0.25f, playerT.position.y, playerT.position.z);
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.layer != 8) return;

        if (this.enabled == false)
        {
            Debug.Log("do not enter");
            collision.transform.position -= transform.forward;
            return;
        }

        player = collision.gameObject;
        playerT = player.GetComponent<Transform>();
        playerYStart = playerT.position.y;
        movScript = player.GetComponent<PlayerMovement>();
        combatScript = player.GetComponent<PlayerCombat>();
        movScript.enabled = false;
        combatScript.rangeOn = false;
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.layer != 8) return;
    //
    //    
    //}

    private void OnTriggerExit(Collider collision)
    {
        if (this.enabled == false) return;

        if (collision.gameObject.layer != 8) return;

        //freePlayer();
    }

    public void freePlayer(bool rage = false)
    {
        if (sfx != null)
        {
            Destroy(sfx);
        }
            
        //if the player kills wall without getting caught, return
        if (player == null) return;

        //playerT.position = new Vector3(playerT.position.x, playerYStart, playerT.position.z);
        //GetComponent<MeshCollider>().enabled = false;
        movScript.enabled = true;
        combatScript.rangeOn = true;
        player = null;
    }

    //private IEnumerator Kill()
    //{
    //    yield return new WaitForSeconds(0.3f);
    //    Destroy(this.gameObject);
    //}
}
