using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class GrabBox : MonoBehaviour
{
    public string attackName;

    public string buttonName;
    public string ButtonName
    {
        get { return buttonName; }
        set
        {
            if (value != null)
            {
                buttonName = value;
            }
            else
            {
                buttonName = "";
            }
        }
    }

    public Fighter fighter;
    public float radius;
    private float attackTimer;
    private bool hasHit;
    private Vector3 ledgePos;
    private int curAttackNo = 0;

    public float attackDurration = 0.2f;
    private int lastAttackNo;


    private void Start()
    {

        fighter = GetComponentInParent<Fighter>();
        radius = GetComponent<SphereCollider>().radius;
    }

    private void Update()
    {
        if (attackTimer != 0 && Time.fixedTime - attackTimer < attackDurration)
        {
            // register hits
            if (Physics.CheckSphere(transform.position, this.radius))
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, this.radius);
                foreach (Collider collider in colliders)
                {
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
                            ledgePos = ledgePos + new Vector3(0.25f,0.125f,0); // slightly right and up
                        } else if (Regex.IsMatch(collider.name, "Left"))
                        {
                            facingLeft = false;
                            ledgePos = ledgePos + new Vector3(-0.25f, 0.125f, 0); // slightly left and up
                        }
                        fighter.Hang(ledgePos, facingLeft);


                    } else
                    {
                        fighter.CanGrabLedge = false;
                    }
                }
            } else
            {
                fighter.CanGrabLedge = false;
            } 
        } 
        else
        {
            // no longer attacking
            attackTimer = 0;
            fighter.CanGrabLedge = false ;
        }
    }

    public void StartAttackTimer()
    {
        attackTimer = Time.fixedTime;

        // only will hit one player per attack       
        hasHit = false;
    }

    private void OnEnable()
    {
        EventManager.StartListening(ButtonName, StartAttackTimer);
    }
    private void OnDisable()
    {
        EventManager.StopListening(ButtonName, StartAttackTimer);
    }



}
