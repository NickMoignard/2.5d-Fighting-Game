using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageForce : MonoBehaviour {

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("OnControllerColliderHit from hitbox called");
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter from hitbox called");
        //foreach (ContactPoint contact in collision.contacts)
        //{
            //Debug.DrawRay(contact.point, contact.normal, 2f, Color.green);
        //}

    }
}
