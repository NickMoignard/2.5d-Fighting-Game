using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class HitCollider : MonoBehaviour {
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

    public int damage;
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public CharacterController controller;
    private float radius;
    private float attackTimer;
    private bool hasHit;
    
    private int curAttackNo = 0;

    public float attackDurration = 0.5f;
    private int lastAttackNo;

    public bool careX;
    public bool careY;

    private void Start()
    {

        controller = GetComponentInParent<CharacterController>();
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
                foreach (Collider hitbox in colliders)
                {

                    
                    
                    if (Regex.IsMatch(hitbox.name, "_Hitbox") && !hasHit )
                    {
                        Debug.Log("enemy hit");
                        


                        Fighter owner = hitbox.GetComponentInParent<Fighter>();
                        CharacterController ownersController = hitbox.GetComponentInParent<CharacterController>();
                        Vector3 direction = controller.transform.position - ownersController.transform.position;
                        Vector3 kickback = Vector3.zero;
                        // set the direction to eminate from fighters center of gravity
    
                        if (direction.x < 0 && careX)
                            //enemy left
                            kickback.x = 1;
                        else if (direction.x > 0 && careX)
                            // enemy right
                            kickback.x = -1;
                        if (direction.y < 0 && careY)
                            //enemy below
                            kickback.y = 1;
                        else if (direction.y > 0 && careY)
                            //enemy above
                            kickback.y = -1;

                        owner.TakeHit(Damage, kickback);
                        hasHit = true;
                    } else
                    {
                        //Debug.Log(hitbox.name);
                    }
                }
            }
        }
        else
        {
            // no longer attacking
            attackTimer = 0;
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
