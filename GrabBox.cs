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
    private float lastLedgeGrabTimeStamp;
    public float GrabCoolDownTime;
    private bool grabDisabled = false;
    public bool GrabDisabled
    {   // when we disable the grab save the time so we can enable it later
        get { return grabDisabled; }
        set
        {
            lastLedgeGrabTimeStamp = Time.fixedTime;
            grabDisabled = value;
        }
    }
    private bool grabbing;
    public bool Grabbing
    {
        get { return grabbing; }
        set
        {
            grabbing = value;
        }
    }

    private void Start()
    {
        fighter = GetComponentInParent<Fighter>();
        radius = GetComponent<SphereCollider>().radius;
    }
    
    private void Update()
    {
        GrabCoolDown();
        if (!grabDisabled && !Grabbing)
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
                        bool facingRight = false;

                        ledgePos = collider.transform.position;

                        // the position of the the ledge is located in center of the collider
                        // we want to grap on the outside edge of the collider
                        if (Regex.IsMatch(collider.name, "Right"))
                        {
                            facingRight = true;

                        }
                        else if (Regex.IsMatch(collider.name, "Left"))
                        {
                            facingRight = false;

                        }
                        fighter.CanGrabLedge = true;
                        lastLedgeGrabTimeStamp = Time.fixedTime;
                        fighter.Hang(ledgePos, facingRight);
                    }
                    else
                    {   // theres no ledges to be grabbed
                        fighter.CanGrabLedge = false;
                    }
                }
            }
        } else if (Grabbing)
        {
            
            Debug.Log("we're on the ledge");
        }
    }

    void GrabCoolDown()
    {   // check to see if ledge grab has cooled down yet
        if (lastLedgeGrabTimeStamp - Time.fixedTime > GrabCoolDownTime)
        {
            GrabDisabled = false;
        }
    }
}
