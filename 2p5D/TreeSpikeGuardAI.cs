using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpikeGuardAI : MonoBehaviour
{
    public string findShoot;
    public string findDeath;

    private GameObject summoner;
    private GameObject shootSound;
    private GameObject deathSound;
    private Animator animator;
    private CapsuleCollider cCollider;
    private float lifeTime;

    void Start()
    {
        summoner = transform.parent.gameObject;
        animator = GetComponent<Animator>();
        cCollider = GetComponent<CapsuleCollider>();
        lifeTime = summoner.GetComponent<EnemyCombat>().iFrames;

        shootSound = GameObject.Find(findShoot);
        deathSound = GameObject.Find(findDeath);

        shootSound = Instantiate(shootSound, transform);
        shootSound.GetComponent<AudioSource>().Play();
    }

    //called by the animation at a specific frame
    public void GetReady()
    {
        //Debug.Log("ready");
        cCollider.enabled = true;
        StartCoroutine(DecayIn(lifeTime));
    }

    //called at the end of the animation
    public void Kill()
    {
        //Debug.Log("Destroyed");
        Destroy(shootSound);
        Destroy(deathSound);
        Destroy(this.gameObject);
    }

    private IEnumerator DecayIn(float s)
    {
        yield return new WaitForSeconds(s);
        deathSound = Instantiate(deathSound, transform);
        deathSound.GetComponent<AudioSource>().Play();
        cCollider.enabled = false;
        animator.SetTrigger("death");
    }
}
