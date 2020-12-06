using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    private GameObject player;
    private PlayerStatus status;
    private PlayerCombat combat;

    public int damage = 1;
    public Vector3 lastDir;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 8)
        {
            return;
        }

        player = collision.gameObject;
        status = player.GetComponent<PlayerStatus>();
        combat = player.GetComponent<PlayerCombat>();
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer != 8)
        {
            return;
        }

        if (status.enabled == false)
        {
            return;
        }

        status.DamagePlayer(damage, 50, combat.GetKnockDir() * -1f);
    }
}
