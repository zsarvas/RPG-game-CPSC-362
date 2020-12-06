using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YRotate : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, speed, 0);
    }
}
