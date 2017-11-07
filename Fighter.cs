using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

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
    private bool movementEnabled = true;
    public bool MovementEnabled
    {
        get { return movementEnabled; }
        set { movementEnabled = value; }
    }
    private Vector3 initSpawnPosition;
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


    // Grabbing
    public Transform HangPosition
    {
        get { return hangPosition; }
        set { hangPosition = value; }
    }
    private Transform hangPosition;


    private bool hanging;
    public bool Hanging
    {
        get { return hanging; }
        set
        {
            Debug.Log("set the hanging flag");
            hanging = value;
        }
    }
    //private bool grabbing;
    //public bool Grabbing
    //{
    //    get { return grabbing; }
    //    set { grabbing = value; }
    //}
    private bool gettingUp;
    public bool GettingUp
    {
        get { return gettingUp; }
        set { gettingUp = value; }
    }
    private float gettingUpTimer;
    public float GettingUpTimer
    {
        get { return gettingUpTimer; }
        set { gettingUpTimer = value; }
    }
    public float GetUpLength = 0.5f;
    public GrabBox grabBox;
    private float grabRadius = 5f;

    public float GrabRadius
    {
        get { return grabRadius; }
        set { grabRadius = value; }
    }
    private bool canGrabLedge;
    public bool CanGrabLedge
    {
        get { return canGrabLedge; }
        set { canGrabLedge = value; }
    }
    private Vector3 ledgePos;
    public Vector3 LedgePos
    {
        get { return ledgePos; }
        set { ledgePos = value; }
    }
    public Vector3 lastMoveVector = Vector3.zero;

    public bool MovingToHang = false;
    // Methods ================================================================================================================================
    private void OnEnable()
    {
        EventManager.StartListening("FwdA", FwdA);
        EventManager.StartListening("Y", Jump);
        EventManager.StartListening("GetUp", GetUpFromHang);
    }
    private void OnDisable()
    {
        EventManager.StopListening("FwdA", FwdA);
        EventManager.StopListening("Y", Jump);
        EventManager.StopListening("GetUp", GetUpFromHang);
    }

    public virtual void Start()
    {   // After loading into scene initialize everthing
        horizontalVelocity = 0;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        character = GetComponent<Character>();
        HangPosition = transform.Find("HangPosition");
        initSpawnPosition = controller.transform.position;
        currentHealth = maximumHealth;
    }

    public virtual void FixedUpdate()
    {



        if (MovementEnabled)
        {
            // Set max downard speed (replicate terminal velocity)
            moveVector.y = Mathf.Max(VerticalVelocity, terminalVelocity);


            // Dont exceeed termninal velocity in either direction on x axis
            if (HorizontalVelocity < 0)
            {
                moveVector.x = Mathf.Max(HorizontalVelocity, terminalVelocity);
            }
            else
            {
                moveVector.x = Mathf.Min(HorizontalVelocity, -terminalVelocity);
            }

            if (jumpVelocity > 0)
            {   // Apply accumulated jump velocity and reset
                VerticalVelocity += JumpVelocity;
                moveVector.y = VerticalVelocity;
                JumpVelocity = 0;
            }
            moveVector = moveVector * Time.deltaTime;


            // negate movement in the Z-axis - 2.5D in one line
            moveVector.z = initSpawnPosition.z - transform.position.z;


            // Apply movement vector
            controller.Move(moveVector);
        }

    }

    void UpdateAnimator()
    {   // check if grounded
        if (controller.isGrounded)
        {
            animator.SetBool("IsGrounded", true);
            animator.SetBool("Falling", false);
        }
        else
        {
            animator.SetBool("IsGrounded", false);
            // falling ?
            if (VerticalVelocity < -1.1f)
            {
                animator.SetBool("Falling", true);
            } else
            {
                animator.SetBool("Falling", false);
            }
        }
    }


    public virtual void Update()
    {
        UpdateAnimator();

        // User input cannot be accurately read with fixeUpdate
        if (MovementEnabled && !GettingUp)
        {
            Gravity();
            ApplyFriction();
        } else if (Hanging)
        {
            Drop();
            VerticalVelocity = 0;

            Vector3 moveV = LedgePos - HangPosition.position;

            
            if (moveV.magnitude < 1.5)
            {
                controller.Move(moveV * Time.deltaTime * moveV.magnitude * 0.01f);
                Debug.Log("1");
            }
            else if (moveV.magnitude < 3)
            {
                controller.Move(moveV * Time.deltaTime * moveV.magnitude * 0.2f);
                Debug.Log("2");
            }
            else
            {
                //controller.SimpleMove(moveV);
                controller.Move(moveV * Time.deltaTime * moveV.magnitude * 5);
                Debug.Log("3");
            }


        }

        else if (GettingUp && GettingUpTimer - Time.fixedTime < -GetUpLength)
        {
            //controller.Move(moveV * Time.deltaTime);
            //controller.SimpleMove(moveV * Time.deltaTime * moveV.magnitude * 10);
        }
    }
    public virtual void Drop() {
        // player or computer can call this function to end the hang
        grabBox.GrabDisabled = true;
        ResetHang();
    }

    public virtual void Gravity()
    {
        if (controller.isGrounded)
        {   // Stop falling and reset jumps
            VerticalVelocity = 0.001f;
            jumpTimer = 0;
            jumpCount = 0;
            animator.SetBool("IsGrounded", true);
        }
        else if (GettingUp)
        {
            VerticalVelocity = 0;
            
        }
        else if (controller.velocity.y < 0)
        {
            VerticalVelocity -= gravity * Time.deltaTime;
        }
        // Constantly increase gravity
        VerticalVelocity -= gravityIncreaseRate;

        if (VerticalVelocity < 2)
        {
            animator.SetBool("IsGrounded", false);
        } else
        {
            animator.SetBool("IsGrounded", true);
        }
        animator.SetFloat("VerticalVelocity", VerticalVelocity);
    }

    public void TakeHit(int damage, Vector3 direction)
    {
        Vector3 kickbackForce = Vector3.zero;

        // Decrease player health   
        currentHealth -= damage;

        // animate player
        animator.SetTrigger("Flinch");

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
    public virtual void Test()
    {
        Debug.Log("Testing if we can grab a ledge:" + CanGrabLedge);
    }

    public virtual void Run()
    {
        // ...
    }

    public virtual void FwdA()
    {
        // ...
        animator.SetTrigger("FwdA");
    }

    public virtual void Hang(Vector3 ledgePos, bool direction)
    {
        if (CanGrabLedge)
        {
            LedgePos = ledgePos;
            MovementEnabled = false;
            animator.SetBool("Hanging", true);
            animator.SetBool("FACING_RIGHT", direction);
            Vector3 moveV = ledgePos - HangPosition.position;
            controller.Move(moveV);
            JumpCount = 0;
            grabBox.Grabbing = true;
        } else
        {
            Debug.Log("cant grab ledge but called hang");
            ResetHang();
        }
    }



    void GetUpFromHang()
    {

        float GettingUpTimer = Time.fixedTime;
        // allow user movement and give move character up
        //float x = FacingRight ? -0.058f : 0.058f;
        MovementEnabled = false;
        animator.SetTrigger("GetUp");

        animator.SetBool("Hanging", false);

    }
    public virtual void ResetHang()
    {   // resets all the flags & timers used to hang
       
        GettingUpTimer = 0;
        animator.SetBool("Hanging", false);
        CanGrabLedge = false;
        MovementEnabled = true;
        Hanging = false;
        grabBox.GrabDisabled = true;
        grabBox.Grabbing = false;
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
    
    

    void OnCollisionEnter(Collision collision)
    {
        if (!Regex.IsMatch(collision.gameObject.name, "[Player,Platform]")) 
            Debug.Log("collision detected with " + collision.gameObject.name);

        if (Regex.IsMatch(collision.gameObject.name, "_Bounds"))
        {

            // ==================================================================================================================================================================== hard coded value
            EventManager.TriggerEvent("Destroy_Player_1");
        }

    }

}
