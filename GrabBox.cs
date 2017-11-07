using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class GrabBox : MonoBehaviour
{
    public Fighter fighter;
    public float radius;
    private bool hasHit;
    private Vector3 ledgePos;

    private void Start()
    {
        fighter = GetComponentInParent<Fighter>();
        radius = GetComponent<SphereCollider>().radius;
    }

    private void Update()
    {
        // check grab box
        if (Physics.CheckSphere(transform.position, this.radius))
        {
            // iterate over everything within the grabbox
            Collider[] colliders = Physics.OverlapSphere(transform.position, this.radius);
            foreach (Collider collider in colliders)
            {
                // if there's a ledge - grab it
                if (Regex.IsMatch(collider.name, "Ledge") && !hasHit)
                {
                    bool facingLeft = false; 
                    Debug.Log("Theres a ledge in range");
                    fighter.CanGrabLedge = true;
                    ledgePos = collider.transform.position;

                    // the position of the the ledge is located in center of the collider
                    // we want to grap on the outside edge of the collider
                    if (Regex.IsMatch(collider.name, "Right"))
                    {
                         facingLeft = true;
                         Debug.Log("regex match right ledge");
                    }
                    else if (Regex.IsMatch(collider.name, "Left"))
                    {
                        facingLeft = false;
                        Debug.Log("regex match right ledge");
                    }
                    fighter.Hang(ledgePos, facingLeft);
                }
                else  // theres no ledges to be grabbed
                {
                    fighter.CanGrabLedge = false;
                }
            }
        }
    }
}
