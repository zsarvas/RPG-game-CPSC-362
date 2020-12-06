using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWallKillZone : MonoBehaviour
{
    public bool rage = false;
    public GameObject treeWallDeath;
    public GameObject rageWallDeath;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag != "KillZone") return;

        GameObject col = collider.gameObject;
        Transform t = collider.transform;

        col.GetComponent<BossWall>().freePlayer(true);
        GameObject deathAni = col.layer == 0 ? rageWallDeath : treeWallDeath;
        Instantiate(deathAni, t.position, t.rotation);
        Destroy(col);

    }

    void OnTriggerStay(Collider collider)
    {
        if (rage) return;

        if (collider.gameObject.layer != 8) return;

        GameObject player = collider.gameObject;
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<PlayerCombat>().rangeOn = true;
    }
}
