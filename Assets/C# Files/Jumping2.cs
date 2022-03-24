using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping2 : MonoBehaviour
{
    float distToGround;
    public float force = 15;
    public float gravity = -200f;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    private bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    private void Update() {
        distToGround = GetComponent<Collider>().bounds.extents.y;
        Rigidbody rb = GetComponent<Rigidbody>();

        if (Input.GetKeyDown(KeyCode.RightShift) && IsGrounded()) {
            rb.AddForce((force * 350) * Vector3.up);
        }

        if(!IsGrounded()) {
            rb.AddForce(gravity * Vector3.up);
        }
     }
}