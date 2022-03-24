using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathplane : MonoBehaviour
{

    Vector3 respawnPoint = new Vector3(2706f, 11361f, -28.4f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        other.transform.position = respawnPoint;
        if(other.GetComponent<Rigidbody>()) {
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
