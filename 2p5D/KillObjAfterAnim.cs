using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillObjAfterAnim : MonoBehaviour
{
    public bool isChild = false;
    public string playSFX;

    private GameObject sfx;

    void Start()
    {
        if (playSFX != "")
        {
            sfx = Instantiate(GameObject.Find(playSFX), transform.position, transform.rotation);
            sfx.GetComponent<AudioSource>().Play();
        }
    }

    public void Kill()
    {
        if (sfx != null)
        {
            Destroy(sfx);
        }

        if (isChild)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
