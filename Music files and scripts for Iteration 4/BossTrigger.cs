using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public GameObject boss;
    public GameObject entranceBlock;
    public GameObject entranceRender;
    public GameObject entranceRoots;
    public GameObject entranceDeath;

    private bool done = false;
    private GameObject mainMusic;
    private GameObject bossMusic;

    void Update()
    {
        if (boss == null && !done)
        {
            done = true;
            entranceBlock.GetComponent<BoxCollider>().enabled = false;
            entranceRender.SetActive(false);
            Instantiate(entranceDeath, entranceBlock.transform.position, entranceBlock.transform.rotation, entranceBlock.transform).transform.localScale = new Vector3(1, 1, 1);
            StartCoroutine(KillIn(3f, entranceBlock));
        }
    }

    void Start()
    {
    mainMusic = GameObject.Find("Music");
    bossMusic = GameObject.Find("BossMusic");
    bossMusic.SetActive(false);
    mainMusic.SetActive(true);
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (boss == null) return;

        bossMusic.SetActive(true);
        mainMusic.SetActive(false);
        boss.GetComponent<TreantBossAI>().enabled = true;
        entranceBlock.SetActive(true);

        if (entranceRoots != null)
        {
            Destroy(entranceRoots);
        }

        this.gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    private IEnumerator KillIn(float t, GameObject g)
    {
        yield return new WaitForSeconds(t);
        bossMusic.SetActive(false);
        mainMusic.SetActive(true);
        Destroy(g);
    }
}
