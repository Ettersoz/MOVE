using System;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

//File name MUST match the class name!!

// this is a newer input package, includes (keyboard, gamepad, mouse, etc.) and is more flexible than the old input system
// using UnityEngine.InputSystem;

// This is an attribute. it is in [], gives unity extra info
// requires that the scripo needs a rigidbody2D
// useful because if the object does not have a rigidbody2D, unity will add one for you 
[RequireComponent(typeof(Rigidbody2D))]


// this is a class declaration, reminds me of C++
// in unity, script = class
// "public"         -> other scripts can see/use this class
// "class"          -> we're definind a new type of thing 
// "PlayerMovement" -> the name of the class we are defining
// "MonoBehaviour"  -> PlayerMovement is a monobehavior, (inherits from it)
// Similar to godots "exends" Inheriting from MonoBehavior lets you attach the "
// Script to game Obects and gives you access to Awake, Update, FixedUpdate, etc.
public class PlayerController : MonoBehaviour
{
    //Variables, also called Fields, declared at the top of class so every method below can use them 
    // Type: Name: = Value;

    //[SerializeField] this shows Private variable in the insepector, allows you to edit it 
    // without editing the code itself. same thing as @export in Godot
    // if float (value)f doesn't have the f C# thinks this is a double, and will give you an error

    [Header("Movement")]
    public float moveSpeed = 8f;
    public float jumpForce = 14f;
    public float initialJumpVelocityX = 0f;
    public float slideForce = 5f;
    public float currentSlideVelocity = 0f;
    public float strafeSpeed = .5f; 
    public float minStrafeSpeed = 4f;
    public bool skid = false;

    [Header("Timers")]
    public float slideDampeningTime = 1.5f; // the time at which the slide dampening increases
    public float slideMaxSpeedTime = .5f;
    public float queueTimer = 0f;
    public float queueDuration = .5f;
    

    [Header("Ground Check")]
    public Transform groundCheck; // empty child object placed at the player 
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer; // set this to your "ground" layer

    // components states need access too 
    public Rigidbody2D Rb { get; private set;}
    public Animator Anim { get; private set;}

    private PlayerState queuedState;

    // State machine + one instance of eatch state
    // states are created once and reused, not re-created on every transition 
    public PlayerStateMachine StateMachine {get; private set;}
    public IdleState Idle {get; private set;}
    public RunState Run {get; private set;}
    public JumpState Jump {get; private set;}
    public FallState Fall {get; private set;}
    public SlideState Slide {get; private set;}
    public SkidState Skid {get; private set;}

    // Input, read once per frame so states don't each poll input
    public float MoveInput { get; private set; } // -1, 0 ,1 
    public bool JumpPressed { get; private set; }
    public bool Sliding { get; private set; }
    
    void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Anim = GetComponentInChildren<Animator>(); // works whether animator is on root or a sprite child

        // build machine and every state, passing in "this" so states can reach player
        StateMachine = new PlayerStateMachine();
        Idle = new IdleState(this, StateMachine);
        Run = new RunState(this, StateMachine);
        Jump = new JumpState(this, StateMachine);
        Fall = new FallState(this, StateMachine);
        Slide = new SlideState(this, StateMachine);
        Skid = new SkidState(this, StateMachine);
    }

    void Start()
    {
        StateMachine.Initialize(Idle);
    }

    void Update()
    {
        HandleInputs();

        // we run logic update to check for any state switches
        StateMachine.CurrentState.LogicUpdate();

        // handles any custom animation differences
        // flip for input, variable jump input, slide input
        StateMachine.CurrentState.VariableAnimationChange();
    }

    // ####################################################
    // INPUT HANDLING FUNCTION(S) 
    // ####################################################

    void HandleInputs()
    {   
        // handles our queue timer completely
        if (queueTimer > 0f)
        {
            Debug.Log("QueueTimer: " + queueTimer);
            Debug.Log("QueuedState: " + queuedState);
            queueTimer -= Time.deltaTime;
            // removes input from queue if it has expired
            if(queueTimer <= 0f) queuedState = null;
        }


        JumpPressed = false;
        Sliding = false;
        skid = false;

        // assign keyboard input to the player controller 
        var kb = Keyboard.current;
        if (kb == null) return;

        MoveInput = 0f;


        // this is where all the inputs will be executed? i believe
        if (kb.aKey.isPressed) MoveInput -= 1f;

        // same for right 
        if (kb.dKey.isPressed) MoveInput += 1f;

        // jump height, set true
        if (kb.spaceKey.wasPressedThisFrame)
        {
            JumpPressed = true;
            TryQueueInput(Jump);
        }

        if (kb.shiftKey.wasPressedThisFrame)
        {
            Sliding = true;
            TryQueueInput(Slide);
        }

        if (kb.wKey.isPressed) skid = true; 

        // once controller support is added, this will be necissary 
        MoveInput = Mathf.Clamp(MoveInput, -1f, 1f);
    }

    // applies whatever states physics have been defined
    void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    // helper function to determine if the player is on the ground 
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    // helper function to determine if the player is falling
    public bool IsFalling()
    {
        return Rb.linearVelocity.y < -.01f;
    }
    
    // 
    public void TryQueueInput(PlayerState state)
    {
    if (!StateMachine.CurrentState.CanQueueInput) return;

        queuedState = state;          // newest press overwrites any older one
        queueTimer = queueDuration;   // and restarts the expiry window    
    }

    public PlayerState useQueuedState()
    {
        Debug.Log("Using Queued State: " + queuedState);
        PlayerState state = queuedState;
        queuedState = null;
        return state;
    }

    // ####################################################
    // STATE SPECIFIC PHYSICS FUNCTIONS 
    // ####################################################

    // phsyics for horizontal movement (running) for the running state
    public void ApplyHorizontalMovement()
    {
        Rb.linearVelocity = new UnityEngine.Vector2(MoveInput * moveSpeed, Rb.linearVelocity.y);
    }

    
    /*
    first block comment W 
    I want to add variable jump height to this game. 
    i assume the way this is done usually is with a continual additional velocity while the button is 
    held down, with an upper and a lower bound. there is a minimum jump height, and a maximum jump height
    and the players timing will determine the hights inbetween
    so we have two variables
     - minmumum jump height 
     - maximum jump height 
     - button press timer
    when the button press is short enough, i.e <= button press timer
     - the player achieves the smallest jump height. 
     - i think the first thing we should do is set up this timing interval. 
    start by setting up a timer variable. 
    */



    // phsyics for vertical movement (jumping) for the jump state
    public void ApplyJumpMovement()
    {
        // check for a Jump Press, as this will run every frame in physicsUpdate
        Rb.linearVelocity = new UnityEngine.Vector2(Rb.linearVelocity.x, jumpForce);
        // initial jump velocity saved for strafing while falling 
        initialJumpVelocityX = Rb.linearVelocity.x; 
    }

    // slide physics for the slide state
    public void ApplySlide(float initialMoveInput, float maxSlideTimer)
    {
        float slideDampening = 1.02f; // the rate at which the slide recreases 
        // increase dampening when slide velocity is low (thoretically)

        if (maxSlideTimer > slideDampeningTime) slideDampening = 1.1f;

        // stop sliding when velocity is very low
        if (currentSlideVelocity <= 1) currentSlideVelocity = 0;
        // apply slide at a current rate right now 

        if (maxSlideTimer > slideMaxSpeedTime) 
        {
            currentSlideVelocity = currentSlideVelocity / slideDampening; // reduce the slide velocity by the dampening value
        }

        Rb.linearVelocity = new UnityEngine.Vector2(initialMoveInput * currentSlideVelocity, Rb.linearVelocity.y);
    }

    // function to allow strafing when falling
    public void ApplyStrafe()
    {
        // determines the strafe cap between the initial horizontal movement speed 
        // and the minimum strafe speed the player should achieve
        float strafeCap = Math.Max(Math.Abs(initialJumpVelocityX), minStrafeSpeed);

        float newVelocityX = 0f;

        // The player can only strafe up to the calculated cap 
        if (Math.Abs(Rb.linearVelocity.x + (MoveInput * strafeSpeed)) > strafeCap) 
            newVelocityX = strafeCap * MoveInput;
        else
            newVelocityX = Rb.linearVelocity.x + (MoveInput * strafeSpeed);
        
        Rb.linearVelocity = new UnityEngine.Vector2(newVelocityX, Rb.linearVelocity.y);
    }

    public void ApplySkid()
    {
        Rb.linearVelocity = new UnityEngine.Vector2(Rb.linearVelocity.x / 1.3f, Rb.linearVelocity.y);
        // if velocity is low enough, just set 0, reduce computation, increase consistency
        if (Math.Abs(Rb.linearVelocity.x) <= 0.1) Rb.linearVelocity = new UnityEngine.Vector2(0, Rb.linearVelocity.y);
    }

    // ####################################################
    // STATE SPECIFIC ANIMATION FUNCTIONS 
    // ####################################################

    public void FlipToFaceInput()
    { 
        // flip based on what direction the player is trying to input movements
        // this needs exclusion/update for slide state           
        if (MoveInput != 0)
            transform.localScale = new UnityEngine.Vector3(Mathf.Sign(MoveInput), 1f, 1f);
    }

    // Draws the ground check circle in the Scene view for debugging. 
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public bool ChangeFallAnimation()
    {
        float threshold = 5f;
        return (Rb.linearVelocity.y < threshold && Rb.linearVelocity.y > 0) || (Rb.linearVelocity.y < -threshold);
    }

}
