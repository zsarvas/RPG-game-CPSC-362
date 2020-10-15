using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPos : MonoBehaviour
{
    public string resetButton = "p";

    private Vector3 startPos;

    void Start() {
        startPos = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(resetButton.ToLower()))
        {
            transform.position = startPos;
        }
    }
}
