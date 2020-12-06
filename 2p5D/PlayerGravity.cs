using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    public Vector3 fallSpeed = new Vector3(0, -100, 0);

    private PlayerMovement movScript;
    private Rigidbody rb;
    private Vector3 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movScript = GetComponent<PlayerMovement>();
    }

    void FixedUpdate()
    {
        movement = movScript.GetMovement();
        StickToGround();
    }

    //enables falling if moving and prevents sliding if still
    void StickToGround()
    {
        if (movement.sqrMagnitude != 0)
        {
            rb.velocity = fallSpeed;
            return;
        }
        rb.velocity = Vector3.zero;
    }
}
