using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerCam : MonoBehaviour
{
    public GameObject player;
    //public Transform camP;
    public float rotation = 10f;

    private GameObject playerObj;
    private Transform camPoint;

    void Start()
    {
        //if (player == null)
        //{
        //    playerObj = GameObject.find("PlayerQuad");
        //    if (playerObj == null)
        //    {
        //        Debug.Log("Error: Camera's default object or set object not found.");
        //        return;
        //    }
        //    player = playerObj.GetComponent<Transform>();
        //}
        camPoint = player.transform.Find("cameraPoint").GetComponent<Transform>();
    }

    void LateUpdate()
    {
        transform.position = camPoint.position;
        transform.rotation = camPoint.rotation;
    }
}
