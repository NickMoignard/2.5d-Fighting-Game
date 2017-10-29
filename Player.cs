using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Fighter
{
    public float LONGPRESSMODIFIER = 0.5f;
    public float MAXBUTTONDOWNTIME = 0.2f;    
    
    
    // - User Input
    public float Left_X
    {
        get { return left_x; }
        set { left_x = value; }
    }
    private float left_x;

    public float Left_Y
    {
        get { return left_y; }
        set { left_y = value; }
    }
    private float left_y;

    override public void Start()
    {
        base.Start();
        animator.SetBool("FACING_RIGHT", FacingRight);
    }

    override public void Update()
    {
        base.Update();

        // Get User Input
        Left_X = Input.GetAxis("Horizontal");
        Left_Y = Input.GetAxis("Vertical");

        animator.SetFloat("x", Left_X);

        if (Left_X < -0.1) // user left
        {
            FacingRight = false;
            animator.SetBool("FACING_RIGHT", FacingRight);
        }
        else if (Left_X > 0.1) // user right
        {
            FacingRight = true;
            animator.SetBool("FACING_RIGHT", FacingRight);
        }

        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump Pressed/nJumps Left: "+ (MaxJumps - JumpCount));

            Jump();
        }
            
        if (Input.GetButtonDown("A"))
        {
            Debug.Log("A Pressed");
            EventManager.TriggerEvent("A");
        }
        Run();

    }

    public override void Run()
    {
        base.Run();

        if ((Left_X > 0.1) || (Left_X < -0.1))
        {
            float x_velocity = Left_X * character.MoveSpeed;
            HorizontalVelocity += x_velocity;
        }

    }

    override public void Gravity()
    {   // override gravity in order to implement Directional Influence
        base.Gravity();

        // Add more jump force when jump held down
        if (controller.velocity.y > 0 && Input.GetButton("Jump"))
        {
            if (Time.fixedTime - JumpTimer < MAXBUTTONDOWNTIME)
            {
                // increase jump velocity with long press of jump button
                JumpVelocity += LONGPRESSMODIFIER;
            }
        }
        else if (controller.velocity.y < 0)
        {
            if (Left_Y > 0)
            {   // Player wants to get to ground - increase fall speed
                VerticalVelocity -= JUMPDIMODIFIER * Left_Y;
            }
        }
    }


}
