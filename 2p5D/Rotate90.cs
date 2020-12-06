using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate90 : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        RotateCheck();
    }

    void RotateCheck()
    {
        //rotate clockwise
        if (Input.GetKeyDown("e"))
        {
            transform.Rotate(0, 90, 0);
        }

        //rotate counter clockwise
        if (Input.GetKeyDown("q"))
        {
            transform.Rotate(0, -90, 0);
        }
    }
}
