using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour {

    public CharacterController controller;
    public Animator animator;
    public Character character;

    // Constants
    public float terminalVelocity = -50f;
    public float gravity = 14f;
    public float gravityIncreaseRate = 1f;
    public float groundFriction = 3f;
    public float kickbackModifier = 2f;
    public float JUMPDIMODIFIER = 5f;
    public float horizontalVelocityDampener = 1f;
    private float frictionModifier = 1.2f;

    public float Friction
    {
        get { return frictionModifier; }
        set { frictionModifier = Mathf.Max(value, 1); }  // Math gets weird
    }
    public const int kickbackModifierNumerator = 0;
    public const int kickbackModifierDenominator = 0;

    // Movement
    private Vector3 spawnPosition;
    private Vector3 moveVector = Vector3.zero;
    public Vector3 MoveVector
    {
        get { return moveVector; }
        set { moveVector = value; }
    }



    private float horizontalVelocity = 0;
    public float HorizontalVelocity
    {
        get { return horizontalVelocity; }
        set { horizontalVelocity = value; }
    }
    private float verticalVelocity = 0;
    public float VerticalVelocity
    {
        get { return verticalVelocity; }
        set { verticalVelocity = value; }
    }

    // unclear if this is a necessary level of protection
    private bool facingRight = true;
    public bool FacingRight
    {
        get { return facingRight; }
        set { facingRight = value; }
    }

    // Jump logic

    public int jumpCount = 0;
    private float jumpTimer;
    private int maxJumps;
    public int MaxJumps
    {   // Simplification of the GUI for character creation
        get { return maxJumps; }
        set { maxJumps = value; }
    }
    public float JumpTimer
    {
        get { return jumpTimer; }
        set { jumpTimer = value; }
    }

    public float jumpVelocity = 0;
    public float JumpVelocity
    {
        get { return jumpVelocity; }
        set { jumpVelocity = value; }
    }

    public int JumpCount
    {
        get
        {
            return jumpCount;
        }

        set
        {
            jumpCount = value;
        }
    }

    // Health
    private int currentHealth;
    public int maximumHealth;


    // Methods ================================================================================================================================

    public virtual void Start()
    {   // After loading into scene initialize everthing
        horizontalVelocity = 0;

        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        character = GetComponent<Character>();

        Debug.Log("Character Name: " + character.CharacterName);
        Debug.Log("Jump size: " + character.JumpSize );
        spawnPosition = controller.transform.position;
        currentHealth = maximumHealth;
    }

    public virtual void FixedUpdate()
    {
        // Set max downard speed (replicate terminal velocity)
        moveVector.y = Mathf.Max(VerticalVelocity, terminalVelocity);


        // Dont exceeed termninal velocity in either direction on x axis
        if (horizontalVelocity < 0)
        {
            moveVector.x = Mathf.Max(horizontalVelocity, terminalVelocity);
        }
        else
        {
            moveVector.x = Mathf.Min(horizontalVelocity, -terminalVelocity);
        }

        if (jumpVelocity > 0)
        {   // Apply accumulated jump velocity and reset
            VerticalVelocity += JumpVelocity;
            moveVector.y = VerticalVelocity;
            JumpVelocity = 0;
        }
        moveVector = moveVector * Time.deltaTime;

        // negate movement in the Z-axis - 2.5D in one line
        moveVector.z = spawnPosition.z - transform.position.z;
        
        // Apply movement vector
        controller.Move(moveVector);
    }

    public virtual void Update()
    {
        // User input cannot be accurately read with fixeUpdate
        Gravity();
        ApplyFriction();
    }

    public virtual void Gravity()
    {
        if (controller.isGrounded)
        {   // Stop falling and reset jumps
            VerticalVelocity = 0;
            jumpTimer = 0;
            jumpCount = 0;
        } else if (controller.velocity.y < 0)
        {
            VerticalVelocity -= gravity * Time.deltaTime;
        }
        // Constantly increase gravity
        VerticalVelocity -= gravityIncreaseRate;
    }

    public void TakeHit(int damage, Vector3 direction)
    {
        Vector3 kickbackForce = Vector3.zero;

        // Decrease player health   
        currentHealth -= damage;

        // create an inversely proportional relationship between health and kickback force
        float kickbackMultiplier = (currentHealth + kickbackModifierNumerator / maximumHealth + kickbackModifierDenominator);
        direction.Normalize(); // ensure input parameter doesn't contain magnitudal information
        kickbackForce = direction * kickbackMultiplier * damage;

        // apply kickback force
        VerticalVelocity += kickbackForce.y;
        horizontalVelocity += kickbackForce.x;
    }

    public virtual void Jump()
    {
        if (jumpCount < MaxJumps)
        {
            animator.SetTrigger("JUMP");  //animate the jump
            jumpCount += 1;  // count
            // stop downwards momentum when jumping
            VerticalVelocity = 0;
            jumpVelocity += character.JumpSize;  // apply force

            jumpTimer = Time.fixedTime;  //time stamp
        } else
        {
            Debug.Log("Its not triggering");
        }
    }

    public virtual void Run()
    {
        // ...
    }

    public void ApplyFriction()
    {   // slow horizontal movement
        if (horizontalVelocity > 10 || horizontalVelocity < -10)
        {   // sharp slow down
            horizontalVelocity -= horizontalVelocity / frictionModifier;
        }
        else
        {
            if (horizontalVelocity > 1 )
            {   // long linear slow down
                horizontalVelocity -= 1;
            }
            else if (horizontalVelocity < -1)
            {
                horizontalVelocity += 1;
            } else
            {
                horizontalVelocity = 0;
            }
        }
        // Where we will check for walls in order add the advanced wall tech mechanics
    }
}
