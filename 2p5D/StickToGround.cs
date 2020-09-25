using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToGround : MonoBehaviour
{
    public Rigidbody rb;
    private Vector3 fallSpeed = new Vector3(0, -100, 0);


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {



        rb.velocity = fallSpeed;

        /*
        //set velocity to -10 in y direction if player is off the ground by 1
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit);
        if (hit.distance < 0.476f)
        {
            Debug.Log("Hit!");
            rb.velocity = new Vector3(0, 0, 0);
        }
        else
        {
            Debug.Log(hit.distance);
            rb.velocity = fallSpeed;
            //rb.MovePosition(rb.position + fallSpeed * Time.fixedDeltaTime);
        }
        */
    }
/*
    void FixedUpdate()
    {



        //rb.velocity = fallSpeed;

        
        //set velocity to -10 in y direction if player is off the ground by 1
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit);
        if (hit.distance < 1f)
        {
            Debug.Log("Hit!");
            rb.velocity = new Vector3(0, 0, 0);
        }
        else {
            Debug.Log(hit.distance);
            rb.velocity = fallSpeed;
            //rb.MovePosition(rb.position + fallSpeed * Time.fixedDeltaTime);
        } 
        
    }
    */
}
