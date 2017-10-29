using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class Attack : MonoBehaviour {
    // This will act as a base class for all our custom attack

    private CharacterController controller;
    private HitCollider[] attackBoxes;
    private Animator animator;
    private float attackTimer = 0;

    public float attackDurration = 0.1f;
    public float AttackDurration
    {
        get { return attackDurration; }
        set { attackDurration = value; }
    }

    public string buttonString;
    public string ButtonString
    {
        get { return buttonString; }
        set { buttonString = value; }
    }

    void Start () {
        controller = GetComponent<CharacterController>();
        attackBoxes = controller.GetComponents<HitCollider>();
        animator = GetComponent<Animator>();
	}

    void Animate()
    {
        animator.SetTrigger(buttonString);
    }

    private void OnEnable()
    {        
        EventManager.StartListening(buttonString, Animate);
    }

    private void OnDisable()
    {
        EventManager.StopListening(buttonString, Animate);
    }
}
