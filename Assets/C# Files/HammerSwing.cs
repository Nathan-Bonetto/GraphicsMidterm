using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerSwing : MonoBehaviour
{
    public float force = 500;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool test = false;
    // Update is called once per frame
    void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        HingeJoint hj = GetComponent<HingeJoint>();
        float angle = hj.angle;

        if(test == false) {
            rb.AddForce(force * Vector3.forward);
        }
        
        if(angle < -55.0f) {
            rb.AddForce(force * Vector3.back);
            test = true;
        }

        if(angle > 55.0f) {
            rb.AddForce(force * Vector3.forward);
            test = true;
        }
        print(angle);
    }
}