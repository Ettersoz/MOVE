using UnityEngine;

//File name MUST match the class name!!

// this is a newer input package, includes (keyboard, gamepad, mouse, etc.) and is more flexible than the old input system
using UnityEngine.InputSystem;

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
public class PlayerMovement : MonoBehaviour
{
    //Variables, also called Fields, declared at the top of class so every method below can use them 
    // Type: Name: = Value;

    //[SerializeField] this shows Private variable in the insepector, allows you to edit it 
    // without editing the code itself. same thing as @export in Godot
    // if float (value)f doesn't have the f C# thinks this is a double, and will give you an error

    [SerializeField] private float speed = 6f;
    // each attatchment of this script will have its own value, changing this one will not 
    // effect the others. 
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // These are empty "slots" that will hold references (pointers) to other COMPONENTS
    // on this same GameObject. They start empty (null) and get filled in Awake() or Start()
    // no "public" or [serializeField] = private + hidden by inspector
    Rigidbody2D rb;     // handles physics: velocity, gravity, collisions, etc.
    Animator animator;  // handles animations and hold parameters like "Speed"
    SpriteRenderer sr;  // draws the sprite, we will use to flip left and right

    // Stores which direction the layer is pressing:
    // -1 = left, 0 = not pressing, 1 = right
    // used in update and use in fixed update
    float moveInput; 

    // Use AWAKE or START 

    //########################
    // Awake: 
    // - Runs as soon as the object is created/loaded.
    // - Runs even if the SCRIPT is disabled (unchecked in the Inspector)
    //   as long as the GameObject itself is active
    // - You can't count on OTHER objects being set up yet, because their 
    //   Awake() may not have run yet. Unity does not promise order between 
    //   objects
    //
    // REMEMBER: grab your own components here and set your own starting 
    //           values, don't talk to other objects yet. 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   
    }  

    //########################
    // Start: 
    // - runs before the objects first update 
    // - only runs if this SCRIPT is enabled. If it starts disabled, it will
    //   wait until it is enabled to run 
    // - Guaranteed to run AFTER every Awake() in the scene has run, so 
    //   other objects have finished their own setup and are ready to be used.
    //
    // REMEMBER: anything that depends on ANOTHER object goes here. 

    //void Start()
    //{
        // Safe: GameManager already ran its awake and is ready
        //manager = FindFirstObjectByType<GameManager>();
        // search for the first object of type GameManager in the scene and store 
        // a reference to it in our manager variable
        //manager.RegisterTimer(this);
        // call the GameManager's RegisterTimer() method and give it a reference 
        // to this script (this)
    //}



    // Update is called once per frame (60fps, 60 times per second)
    // similar to process(delta)
    
    // Use Update for: reading input, animations, timers anything visual
    // Don't change physics here, that goes in FixedUpdate()

    void Update()
    {
       // Grab the current keyboard. "Var" means "C#. figure out the 
       // type for me" It's still strongly typed, you just don't have
       // to write it out. 
        var kb = keyboard.current;

        // if there is no keyboard plugged in, kb will be null. Trying to use 
        // a null variable crasshes with an error, so we want to write failsafes 
        // for this instance 
        if (kb == null) return;

        // reset to zero every frame. then we add / subtract based on what keys are pressed
        moveInput = 0f;

        // .ispressed = true for EVERY frame the key is held down
        if (kb.aKey.ispPressed) moveInput -= 1f;

        // same for right 
        if (kb.dKey.isPressed) moveInput += 1f;

        // send a value to the "Speed" parameter in the animator,
        // your transitions check this: Speed > 0.1 -> run, speed < 0.1 -> idle
        //
        // Mathf = Unity's math helper class. Mathf.abs = absolute value
        // turns -1 into 1. Speed should always be positive
        anim.SetFloat("Speed", Mathf.Abs(moveInput));

        // flip the sprite to face the direction we are moving 
        if (moveInput != 0f) sr.flipX = moveInput < 0f;
    }
}
