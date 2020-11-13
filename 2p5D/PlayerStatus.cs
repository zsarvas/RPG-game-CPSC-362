using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    private GameObject[] hearts;
    private bool invincible = false;

    public int maxHealth;
    public int currHealth;
    public float iFrames = .5f;

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
    }

    //use negative values to heal
    public void DamagePlayer(int damage)
    {
        if (damage > 0)
        {
            if (invincible)
            { 
                return;
            }
            StartCoroutine(IFramesStart());
        }

        currHealth -= damage;
        //todo play hurt sound
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
        this.enabled = false;
    }
}
