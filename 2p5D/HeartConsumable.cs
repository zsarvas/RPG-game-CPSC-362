using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartConsumable : MonoBehaviour
{
 
    [SerializeField]
    public int RotateSpeed;
    public AudioSource _CollectSound;
    public bool isQuad = false;
    public bool respawn = false;
    public float secondsTillRespawn = 1f;

    void FixedUpdate(){
        RotateSpeed=2;
        transform.Rotate(0,RotateSpeed,0,Space.World);
    }

    void OnTriggerEnter(Collider other){
        //return if not player
        if (other.gameObject.layer != 8)
        {
            return;
        }

        GameObject player = other.gameObject;
        PlayerStatus status = player.GetComponent<PlayerStatus>();

        //do not consume if player is at max health
        if (status.currHealth == status.maxHealth)
        {
            return;
        }

        _CollectSound.Play();

        if (status.currHealth == 0)
        {
            status.enabled = true;
            Debug.Log("Player has revived");
        }
        
        HealthManagment.HealthValue+=1;
        status.DamagePlayer(-1, 0, Vector3.zero);

        ChangeStateTo(false);

        float t = respawn ? secondsTillRespawn : 1f;
        StartCoroutine(Wait(t));
    }

    //delayed destruction to allow sound or respawning
    private IEnumerator Wait(float time){
        yield return new WaitForSeconds(time);
        if (!respawn)
        {
            Destroy(gameObject);
            yield return 0;
        }
        ChangeStateTo(true);
    }

    //Toggle enabled state of essential components
    void ChangeStateTo(bool b)
    {
        GetComponent<SphereCollider>().enabled = b;
        if (isQuad)
        {
            foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>())
            {
                r.enabled = b;
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = b;
        }
    }




}
