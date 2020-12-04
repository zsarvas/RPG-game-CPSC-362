using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCut : MonoBehaviour
{
    public GameObject Camera1;
    public GameObject Camera2;
    public GameObject Camera3;
    public GameObject FadeOut;
    public GameObject FadeIn;
    public GameObject ThePlayer;

    void Start()
    {
        StartCoroutine(CutSceneStart()); 
    }

    IEnumerator CutSceneStart()
    {
        yield return new WaitForSeconds(5);
        Camera2.SetActive(true);
        Camera1.SetActive(false);
        FadeIn.SetActive(false);
        yield return new WaitForSeconds(10);
        Camera3.SetActive(true);
        Camera2.SetActive(false);
        yield return new WaitForSeconds(9);
        FadeOut.SetActive(true);
        yield return new WaitForSeconds(1);
        ThePlayer.SetActive(true);
        FadeIn.SetActive(true);
        FadeOut.SetActive(false);
        Camera3.SetActive(false);
        FadeIn.SetActive(false);
    }
}
