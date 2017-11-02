using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class OneWayPlatform : MonoBehaviour {
    public Collider platform;

    // Use this for initialization
    void Start () {
        platform = GetComponentInParent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collsion)
    {
       


    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("We're hitting our head on the platform");
        Physics.IgnoreCollision(other, platform, true);
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("We made it through the platform");
        Physics.IgnoreCollision(other, platform, false);
    }
}
