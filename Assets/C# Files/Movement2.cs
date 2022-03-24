using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2 : MonoBehaviour
{
    private float distToGround;
    public float force = 25;
    public float airForce = .2f;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
    }

    private bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.3f);
    }

    private void Update() {
        distToGround = GetComponent<Collider>().bounds.extents.y;
        Rigidbody rb = GetComponent<Rigidbody>();

        if(rb.velocity.magnitude > 1) {
            rb.transform.rotation = Quaternion.LookRotation(new Vector3(rb.velocity.x, 0, rb.velocity.z), transform.up);
        }

        if(!IsGrounded()) {
            rb.drag = 0;
        } else {
            rb.drag = 3;
        }

        if (Input.GetKey(KeyCode.J) && IsGrounded()) {
            rb.AddForce(force * Vector3.left);
        } else if (Input.GetKey(KeyCode.J) && !IsGrounded()) {
            rb.AddForce((force * airForce) * Vector3.left);
        }

        if (Input.GetKey(KeyCode.L) && IsGrounded()) {
            rb.AddForce(force * Vector3.right);
        } else if (Input.GetKey(KeyCode.L) && !IsGrounded()) {
            rb.AddForce((force * airForce) * Vector3.right);
        }

        if (Input.GetKey(KeyCode.I) && IsGrounded()) {
            rb.AddForce(force * Vector3.forward);
        } else if (Input.GetKey(KeyCode.I) && !IsGrounded()) {
            rb.AddForce((force * airForce) * Vector3.forward);
        }

        if (Input.GetKey(KeyCode.K) && IsGrounded()) {
            rb.AddForce(force * Vector3.back);
        } else if (Input.GetKey(KeyCode.K) && !IsGrounded()) {
            rb.AddForce((force * airForce) * Vector3.back);
        }
    }
}