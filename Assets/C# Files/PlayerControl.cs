using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerControl : MonoBehaviour
{
    ////////////////////////////////////////////////////
    // Player movement variables
    ////////////////////////////////////////////////////
    public float force = 25;
    public float airForce = .2f;
    float distToGround;
    bool leapReady;
    bool canHover;
    float time = 0.0f;
    float timeLimit = 0;

    ////////////////////////////////////////////////////
    // Player jumping variables
    ////////////////////////////////////////////////////
    public float jumpForce = 15;
    public float gravity = -200f;
    bool jumpReady = true;

    ////////////////////////////////////////////////////
    // Audio variables
    ////////////////////////////////////////////////////
    public AudioSource jump;
    public AudioSource dive;
    public AudioSource fluud;
    public AudioSource walljump;

    ////////////////////////////////////////////////////
    // Functions
    ////////////////////////////////////////////////////
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
            rb.AddForce(jumpForce * 300 * Vector3.up);
            // Adds force in the opposite direction of the wall
            rb.AddForce(4000 * -Vector3.left);
            return true;
        } else if(Physics.Raycast(transform.position, Vector3.forward, distToGround + 1f)) {
            rb.velocity = Vector3.zero;
            rb.AddForce(jumpForce * 300 * Vector3.up);
            rb.AddForce(4000 * -Vector3.forward);
            return true;
        } else if(Physics.Raycast(transform.position, -Vector3.forward, distToGround + 1f)) {
            rb.velocity = Vector3.zero;
            rb.AddForce(jumpForce * 300 * Vector3.up);
            rb.AddForce(4000 * Vector3.forward);
            return true;
        } else if(Physics.Raycast(transform.position, Vector3.left, distToGround + 1f)) {
            rb.velocity = Vector3.zero;
            rb.AddForce(jumpForce * 300 * Vector3.up);
            rb.AddForce(4000 * -Vector3.left);
            return true;
        }

        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        distToGround = GetComponent<Collider>().bounds.extents.y;

        ////////////////////////////////////////////////////
        // Movement
        ////////////////////////////////////////////////////
        // Face direction of velocity
        if(rb.velocity.magnitude > 1) {
            rb.transform.rotation = Quaternion.LookRotation(new Vector3(rb.velocity.x, 0, rb.velocity.z), transform.up);
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

        ////////////////////////////////////////////////////
        // F.L.U.D.D
        ////////////////////////////////////////////////////
        // Starts F.L.U.D.D timer
        if(Input.GetKey(KeyCode.Space) && !IsGrounded() && canHover) {
            time += Time.deltaTime;
            timeLimit = time + 2;
            // Makes sure timer does not reset until player lands
            canHover = false;
        }

        // Controls F.L.U.D.D based on timer
        if(Input.GetKey(KeyCode.Space) && !IsGrounded()) {
            time += Time.deltaTime;
            if(time < timeLimit) {
                // Kill only vertical velocity, allows player to still move on the x and z axis
                Vector3 vel = rb.velocity;
                vel.y = 0;
                rb.velocity = vel;

                // Plays F.L.U.D.D audio
                if(!fluud.isPlaying) {
                    fluud.Play();
                }
            }
        }

        // Reset timer when player touches the ground
        if(IsGrounded()) {
            canHover = true;
        }

        ////////////////////////////////////////////////////
        // Jumping
        ////////////////////////////////////////////////////
        // Controls drag while in the air
        if(!IsGrounded()) {
            rb.drag = 0;
        } else {
            rb.drag = 3;
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.LeftShift) && IsGrounded()) {
            // Plays jump audio
            jump.Play();
            rb.AddForce((jumpForce * 350) * Vector3.up);
        }

        // Walljump
        if(Input.GetKeyDown(KeyCode.LeftShift) && !IsGrounded()) {
            jumpReady = IsWall(rb);
            // Plays jumping audio
            if(jumpReady) {
                walljump.Play();
            }
        }

        // Dive
        if(!IsGrounded() && Input.GetKey(KeyCode.E) && leapReady) {
            // Plays diving audio
            dive.Play();
            // Kills all movement before preforming a dive
            rb.velocity = Vector3.zero;
            // Adds a slight upwards force
            rb.AddForce(20 * 200 * Vector3.up);
            // Lunges player forward
            rb.AddForce(4000 * transform.forward);
            // Ensures no infinite diving
            leapReady = false;
        } else if(IsGrounded() && !leapReady) {
            leapReady = true;
        }

        // Applies gravity to bring object down when in the air
        if(!IsGrounded()) {
            rb.AddForce(gravity * Vector3.up);
        }
    }
}
