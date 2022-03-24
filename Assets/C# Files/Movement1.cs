using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement1 : MonoBehaviour
{
    float distToGround;
    public float force = 25;
    public float airForce = .2f;
    bool leapReady;
    bool canHover;
    float time = 0.0f;
    float timeLimit = 0;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

    // Checks if object is on the ground
    private bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.3f);
    }

    private void Update() {
        distToGround = GetComponent<Collider>().bounds.extents.y;
        Rigidbody rb = GetComponent<Rigidbody>();
        
        // Face direction of velocity
        if(rb.velocity.magnitude > 1) {
            rb.transform.rotation = Quaternion.LookRotation(new Vector3(rb.velocity.x, 0, rb.velocity.z), transform.up);
        }

        // Dive
        if(!IsGrounded() && Input.GetKey(KeyCode.E) && leapReady) {
            // Kills all movement before preforming a dive
            rb.velocity = Vector3.zero;
            rb.AddForce(20 * 200 * Vector3.up);
            rb.AddForce(4000 * transform.forward);
            leapReady = false;
        } else if(IsGrounded() && !leapReady) {
            leapReady = true;
        }

        // Controls drag while in the air
        if(!IsGrounded()) {
            rb.drag = 0;
        } else {
            rb.drag = 3;
        }

        // Starts F.L.U.U.D timer
        if(Input.GetKey(KeyCode.Space) && !IsGrounded() && canHover) {
            time += Time.deltaTime;
            timeLimit = time + 2;
            // Makes sure timer does not reset until player lands
            canHover = false;
        }

        // Controls F.L.U.U.D based on timer
        if(Input.GetKey(KeyCode.Space) && !IsGrounded()) {
            time += Time.deltaTime;
            if(time < timeLimit) {
                // Kill only vertical velocity, allows player to still move on the x and z axis
                Vector3 vel = rb.velocity;
                vel.y = 0;
                rb.velocity = vel;
            }
        }

        // Reset timer when player touches the ground
        if(IsGrounded()) {
            canHover = true;
        }

        // Basic movement, less movement force when the player is in the air
        if (Input.GetKey(KeyCode.A) && IsGrounded()) {
            rb.AddForce(force * Vector3.left);
        } else if (Input.GetKey(KeyCode.A) && !IsGrounded()) {
            rb.AddForce((force * airForce) * Vector3.left);
        }

        if (Input.GetKey(KeyCode.D) && IsGrounded()) {
            rb.AddForce(force * Vector3.right);
        } else if (Input.GetKey(KeyCode.D) && !IsGrounded()) {
            rb.AddForce((force * airForce) * Vector3.right);
        }

        if (Input.GetKey(KeyCode.W) && IsGrounded()) {
            rb.AddForce(force * Vector3.forward);
        } else if (Input.GetKey(KeyCode.W) && !IsGrounded()) {
            rb.AddForce((force * airForce) * Vector3.forward);
        }

        if (Input.GetKey(KeyCode.S) && IsGrounded()) {
            rb.AddForce(force * Vector3.back);
        } else if (Input.GetKey(KeyCode.S) && !IsGrounded()) {
            rb.AddForce((force * airForce) * Vector3.back);
        }
    }
}