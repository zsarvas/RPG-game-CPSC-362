using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    private GameObject[] hearts;
    private bool invincible = false;
    private AudioSource hurtSound;
    private AudioSource deathSound;
    private Rigidbody rb;

    public int maxHealth;
    public int currHealth;
    public float iFrames = 1f;
    public bool infiniteHealth = false;

    // Start is called before the first frame update
    void Start()
    {
        hearts = new GameObject[maxHealth];

        for(int i = 0; i < hearts.Length ; i++)
        {
            hearts[i] = GameObject.Find("ViewHeart" + (i+1));
            if (hearts[i] == null)
            {
                Debug.Log("heart " + (i+1) + " is null");
            }
        }

        UpdateHpBar();
        hurtSound = GameObject.Find("PlayerHurt").GetComponent<AudioSource>();
        deathSound = GameObject.Find("PlayerDeath").GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    //use negative values to heal
    public void DamagePlayer(int damage, float force, Vector3 forceDir)
    {
        if (damage >= 0)
        {
            if (currHealth == 0) return;

            if (invincible)
            { 
                return;
            }

            //play the hurt sound if not dying
            if (damage < currHealth)
            {
                hurtSound.Play();
            }

            rb.AddForce(forceDir * force, ForceMode.Impulse);
            StartCoroutine(IFramesStart());
        }

        if (infiniteHealth) return;

        currHealth -= damage;
        UpdateHpBar();

        if (currHealth <= 0)
        {
            DeathSequence();
        }
    }

    //controls how many hearts are enabled in the canvas
    void UpdateHpBar()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null)
            {
                return;
            }
            hearts[i].GetComponent<RawImage>().enabled = currHealth >= i+1 ? true : false;
        }
    }

    private IEnumerator IFramesStart()
    {
        invincible = true;
        yield return new WaitForSeconds(iFrames);
        invincible = false;
    }

    //placeholder death sequence
    void DeathSequence()
    {
        Debug.Log("Player has died");
        deathSound.Play();
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerCombat>().enabled = false;
        Animator ani = GetComponent<Animator>();
        ani.SetTrigger("Death");
        ani.SetBool("Idle", true);
        ani.SetBool("Running", false);
        ani.SetFloat("LastDirection", 4f);
        //todo destruction/respawn
        this.enabled = false;
    }
}
