using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyContactDamage : MonoBehaviour
{
    public int damage = 1;

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer != 8)
        {
            return;
        }

        GameObject player = collision.gameObject;
        if (player.GetComponent<PlayerStatus>().enabled == false)
        {
            return;
        }

        player.GetComponent<PlayerStatus>().DamagePlayer(damage);
    }
}
