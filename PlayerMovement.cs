using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private CharacterController controller;
    private Vector3 moveVector;
    private float verticalVelocity;
    private int noJumps;
    private float lastJumpTime;
    private bool isJumping;

    public float moveSpeed = 5;
    public float gravity = 14;
    public int maxJumps = 2;


    void Start () {
        controller = GetComponent<CharacterController>();
	}
	
	void FixedUpdate () {
        Gravity();

        // Create movement vector
        moveVector = Vector3.zero;
        moveVector.x = Input.GetAxis("Horizontal") * moveSpeed;
        moveVector.y = verticalVelocity;
        moveVector.z =  -transform.position.z;  // keep the player locked to the z axis
        controller.Move(moveVector * Time.deltaTime);
	}

    void Gravity()
    {
        // check jump timer and reset
        if (lastJumpTime - Time.fixedTime < -0.2 )
        {
            isJumping = false;
        }


        // constant gravity holding player to ground
        if (controller.isGrounded && !isJumping)
        {
            verticalVelocity = -1;
            
        // apply falling gravity (terminal velocity)
        } else 
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
    }

    private void Jump()
    {
        if (controller.isGrounded)  // if player standing om ground
        {
            noJumps = 1; // first jump
            isJumping = true;
            verticalVelocity = 10f;  // apply jump force
            lastJumpTime = Time.fixedTime; // start timer

        } else if (noJumps <= maxJumps)
        {
            noJumps = noJumps + 1; // add additional jump
            isJumping = true;
            verticalVelocity = 10f;
            lastJumpTime = Time.fixedTime;
        }
    }


    // Listen for Input Manager events
    private void OnEnable()
    {
        EventManager.StartListening("jump", Jump);
    }
    private void OnDisable()
    {
        EventManager.StopListening("jump", Jump);
    }
}
