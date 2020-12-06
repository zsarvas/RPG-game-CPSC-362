using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpikeAI : MonoBehaviour
{
    public int damage = 1;
    public string findStart;
    public string findShoot;
    public string findDeath;

    private GameObject startSound;
    private GameObject shootSound;
    private GameObject deathSound;
    private GameObject summoner;

    private CapsuleCollider cCollider;

    void Start()
    {
        startSound = GameObject.Find(findStart);
        shootSound = GameObject.Find(findShoot);
        deathSound = GameObject.Find(findDeath);
        summoner = transform.parent.gameObject;
        cCollider = GetComponent<CapsuleCollider>();

        startSound = Instantiate(startSound, transform);
        startSound.GetComponent<AudioSource>().Play();
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.layer != 8) return;

        collider.gameObject.GetComponent<PlayerStatus>().DamagePlayer(damage, 0f, Vector3.zero);
    }

    //called by the animation at a specific frame
    public void GetReady()
    {
        //Debug.Log("ready");
        shootSound = Instantiate(shootSound, transform);
        shootSound.GetComponent<AudioSource>().Play();
        summoner.GetComponent<BossSpikeTreeAI>().lockedOn = false;
        cCollider.enabled = true;
    }

    //called by the animation at a specific frame
    public void Done()
    {
        //Debug.Log("done");
        cCollider.enabled = false;
    }

    //called at the end of the animation
    public void Kill()
    {
        //Debug.Log("Destroyed");
        Destroy(startSound);
        Destroy(shootSound);
        Destroy(deathSound);
        Destroy(this.gameObject);
    }

    public void PlaySFX(int i)
    {
        switch (i)
        {
            case 0:
                //Debug.Log("start");
                break;
            case 1:
                deathSound = Instantiate(deathSound, transform);
                deathSound.GetComponent<AudioSource>().Play();
                break;
            default:
                Debug.Log("error: " + name + " PlaySFX() " + i);
                break;
        }
    }
}
