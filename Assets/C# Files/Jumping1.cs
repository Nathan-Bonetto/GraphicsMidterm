using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping1 : MonoBehaviour
{
    float distToGround;
    public float force = 15;
    public float gravity = -200f;
    bool jumpReady = true;

    // Start is called before the first frame update
    void Start()
    {
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    // Checks if object is on the ground
    private bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    // Checks for walls to walljump off of
    private bool IsWall(Rigidbody rb) {
        if(Physics.Raycast(transform.position, -Vector3.left, distToGround + 1f)) {
            // Stops all velocity
            rb.velocity = Vector3.zero;
            // Jumping force
            rb.AddForce(force * 300 * Vector3.up);
            // Adds force in the opposite direction of the wall
            rb.AddForce(4000 * -Vector3.left);
            return true;
        } else if(Physics.Raycast(transform.position, Vector3.forward, distToGround + 1f)) {
            rb.velocity = Vector3.zero;
            rb.AddForce(force * 300 * Vector3.up);
            rb.AddForce(4000 * -Vector3.forward);
            return true;
        } else if(Physics.Raycast(transform.position, -Vector3.forward, distToGround + 1f)) {
            rb.velocity = Vector3.zero;
            rb.AddForce(force * 300 * Vector3.up);
            rb.AddForce(4000 * Vector3.forward);
            return true;
        } else if(Physics.Raycast(transform.position, Vector3.left, distToGround + 1f)) {
            rb.velocity = Vector3.zero;
            rb.AddForce(force * 300 * Vector3.up);
            rb.AddForce(4000 * -Vector3.left);
            return true;
        }

        return false;
    }

    private void Update() {
        Rigidbody rb = GetComponent<Rigidbody>();

        // Jumping
        if (Input.GetKeyDown(KeyCode.LeftShift) && IsGrounded()) {
            rb.AddForce((force * 350) * Vector3.up);
        }

        // Walljump
        if(Input.GetKeyDown(KeyCode.LeftShift) && !IsGrounded()) {
            jumpReady = IsWall(rb);
        }

        // Enables walljump again after touching ground
        if(IsGrounded() && !jumpReady) {
            jumpReady = true;
        }

        // Applies gravity to bring object down when in the air
        if(!IsGrounded()) {
            rb.AddForce(gravity * Vector3.up);
        }
     }
}