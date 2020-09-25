using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapFPS : MonoBehaviour
{
    public int cap = 60;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = cap;    
    }

}
