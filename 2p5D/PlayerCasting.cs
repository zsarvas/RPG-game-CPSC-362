using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCasting : MonoBehaviour
{
    public static float _DistancefromTarget;
    public float _ToTarget;
    private PlayerMovement movScript;

    void Start()
    {
        movScript = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast (transform.position, transform.TransformDirection(GetDirVector()), out hit)){
            Debug.DrawRay(transform.position, transform.TransformDirection(GetDirVector()) * hit.distance, Color.green);
            _ToTarget=hit.distance;
            _DistancefromTarget= _ToTarget;
        }
    }

    //get direction vector based on lastDirection float
    public Vector3 GetDirVector()
    {
        switch (movScript._lastDirection)
        {
            case 0:
                return Vector3.forward;
            case 1:
                return Vector3.right;
            case 2:
                return Vector3.back;
            case 3:
                return Vector3.left;
            default:
                Debug.Log("error: player casting entered default.");
                return Vector3.forward;
        }
    }
}
