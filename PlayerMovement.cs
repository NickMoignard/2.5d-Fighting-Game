using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private CharacterController controller;
    private Vector3 moveVector;
    private float verticalVelocity;
    private int noJumps;
    public int maxJumps = 2;
    private BoxCollider feet;
    private bool isJumping;
    public Collider[] attackHitboxes;
    public float moveSpeed = 5;
    private float lastJumpTime;


	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
        feet = GetComponent<BoxCollider>();


        Debug.Log("the logger works");
	}
	


	
	void FixedUpdate () {
        Gravity();

        

        moveVector = Vector3.zero;
        moveVector.x = Input.GetAxis("Horizontal") * moveSpeed;
        moveVector.y = verticalVelocity;

        controller.Move(moveVector * Time.deltaTime);
	}

    void Gravity()
    {
        // check jump timer and reset
        Debug.Log(isJumping);

        if (lastJumpTime - Time.fixedTime < -0.2 )
        {
            isJumping = false;
        }

        if (controller.isGrounded && !isJumping)
        {
            verticalVelocity = -1;
            

        } else 
        {
            verticalVelocity -= 14 * Time.deltaTime;
        }
    }

    private void Jump()
    {

        
        if (controller.isGrounded)
        {
            noJumps = 0;
            isJumping = true;
            verticalVelocity = 10f;
            lastJumpTime = Time.fixedTime; // start timer
        } else if (noJumps <= maxJumps)
        {
            isJumping = true;
            verticalVelocity = 10f;
            lastJumpTime = Time.fixedTime; // start timer
        }
    }

    private void OnEnable()
    {
        EventManager.StartListening("jump", Jump);
    }
    private void OnDisable()
    {
        EventManager.StopListening("jump", Jump);
    }
}
