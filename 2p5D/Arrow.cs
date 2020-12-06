using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage = 1;
    public float knockback = 0f;

    void Start()
    {
        StartCoroutine(SelfDestruct(3f));
    }

    private void OnTriggerEnter(Collider collider)
    {
        GameObject obj = collider.gameObject;
        LayerMask layer = obj.layer;
        //Debug.Log("Arrow hit layer: " + LayerMask.LayerToName(layer));

        //check if object is enemy
        if (layer.value == 9)
        {
            obj.GetComponent<EnemyCombat>().DamageEnemy(damage, knockback, transform.up, 0f);
        }

        //go through object if it is ignored by raycast
        if (layer.value != 2 && layer.value != 10)
        {
            Destroy(gameObject);
        }
    }

    //auto destroy arrow after 3 seconds in the air
    private IEnumerator SelfDestruct(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
