using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public GameObject[] drops;
    public GameObject deathAnimation;
    public AudioSource hurtSound;
    public AudioSource deathSound;
    public bool notWall = true;
    public int maxHP = 3;
    public int currHP;
    public float iFrames = 1f;
    public float deathTime = 0;
    public float exp = 0;
    public float knockbackResistance = 100f;
    public float[] immuneTo;
    public float[] dropChance;
    public string findHurt;
    public string findDeath;
    public bool deathAnimationIsChild = true;

    private Rigidbody rb;
    private bool invincible;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameObject hurt;
        GameObject death;
        invincible = false;

        if (hurtSound == null && findHurt != "")
        {
            if ((hurt = GameObject.Find(findHurt)) != null)
            {
                hurtSound = hurt.GetComponent<AudioSource>();
            }
        }

        if (deathSound == null && findDeath != "")
        {
            if ((death = GameObject.Find(findDeath)) != null)
            {
                deathSound = death.GetComponent<AudioSource>();
            }
        }
    }

    public void DamageEnemy(int dmg, float force, Vector3 forceDir, float dmgType)
    {
        if (dmg >= 0)
        {
            if (invincible)
            {
                return;
            }

            foreach (float t in immuneTo)
            {
                if (t == dmgType)
                {
                    //play different hurt sound?
                    return;
                }
            }

            //play the hurt sound if not dying
            if (hurtSound != null && dmg < currHP)
            {
                hurtSound.Play();
            }

            rb.AddForce(forceDir * GetKnockback(force), ForceMode.Impulse);
            StartCoroutine(IFramesStart());
        }

        currHP -= dmg;

        Debug.Log(name + " -" + dmg + " hp. " + currHP + "/" + maxHP);

        if (currHP <= 0)
        {
            Debug.Log(name + " has died.");

            if (deathSound != null)
            {
                deathSound.Play();
            }

            if (deathAnimation != null)
            {
                if (deathAnimationIsChild)
                {
                    Instantiate(deathAnimation, transform.position, transform.rotation, this.transform);
                }
                else
                {
                    Instantiate(deathAnimation, transform.position, transform.rotation);
                }
            }

            //make sure to turn off enemy's AI script from itself

            if (notWall)
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                if (GetComponent<Animator>() != null)
                {
                    GetComponent<Animator>().enabled = false;
                }

                GetComponent<BoxCollider>().enabled = false;
            }

            foreach (MeshRenderer render in GetComponentsInChildren<MeshRenderer>())
            {
                render.enabled = false;
            }

            if (drops.Length != 0)
            {
                EnemyDrops();
            }

            StartCoroutine(DestroyAfterSeconds(deathTime));
        }
    }

    private IEnumerator DestroyAfterSeconds(float seconds) {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }

    float GetKnockback(float playerForce)
    {
        float net = playerForce - knockbackResistance;

        if (net > 0f) return net;

        return 0f;
    }

    private IEnumerator IFramesStart()
    {
        invincible = true;
        yield return new WaitForSeconds(iFrames);
        invincible = false;
    }

    void EnemyDrops()
    {
        int i = 0;
        Vector3 spawnPos = transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            spawnPos = transform.position - new Vector3(0, hit.distance, 0);
        }

        foreach (GameObject drop in drops)
        {
            if (Random.Range(0, 100) <= dropChance[i])
            {
                Instantiate(drop, spawnPos, transform.rotation);
            }
            i += 1;
        }
    }
}
