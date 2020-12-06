using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAttackRange : MonoBehaviour
{
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetComponent<Transform>().position, transform.root.GetComponent<PlayerCombat>().attackRange);
    }
}
