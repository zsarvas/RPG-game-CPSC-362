using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGem : MonoBehaviour
{
    public AudioSource pickUpSFX;
    public bool bossTest = false;
    public string interactButton = "t";

    private Transform obj;
    private GameObject text;

    //for boss testing
    private string bossTesttxt = "Enter Boss Area?";
    //

    void Start()
    {
        if (pickUpSFX == null)
        {
            pickUpSFX = GameObject.Find("PickUp").GetComponent<AudioSource>();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        //return if not player
        if (collider.gameObject.layer != 8) return;

        obj = collider.gameObject.transform.Find("text");
        text = obj.gameObject;

        if (bossTest)
        {
            text.GetComponent<TextMesh>().text = bossTesttxt;
        }

        text.SetActive(true);
    }

    void OnTriggerStay(Collider collider)
    {
        //return if not player
        if (collider.gameObject.layer != 8) return;

        if (Input.GetKeyDown(interactButton.ToLower()))
        {
            text.SetActive(false);
            pickUpSFX.Play();

            if (!bossTest)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Transform player = obj.parent.transform;
                Transform test = GameObject.Find("BossTestStartPoint").transform;
                player.gameObject.GetComponent<PlayerCombat>().meleeOn = true;
                player.position = test.position;
                player.rotation = test.rotation;
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        //return if not player
        if (collider.gameObject.layer != 8) return;

        text.SetActive(false);
    }
}
