using UnityEngine;


// class inherits from the PlayerState class
public class IdleState : PlayerState
{

    // this calls the constructor for the PlayerState class and initializes it with 
    // player controller p and PlayerStateMachine m,
    // the key word "base" refers to the base object "PlayerState" 
    public IdleState(PlayerController p, PlayerStateMachine m) : base(p, m) {}

    float idleTimer = 0f;

    float randInterval = 0f;

    public string[] IdleAnims = { "Idle0", "Idle1", "Idle2", "Idle3" };

    string currentAnim = "Idle0";


    // the animator must be named exactly "Idle" 
    // the override keyword "overrides" the definition of the function in the superclass
    // PlayerState. This allows us to make specific changes to the class function 
    // base on the subclass we are making
    public override void Enter()
    {
        player.Anim.Play("Idle0"); 
        randInterval = Random.Range(3f, 4f); 
    } 

    public override void LogicUpdate()
    {
        // if not touching the ground -> we are falling, i.e switch states
        if (!player.IsGrounded())
            machine.ChangeState(player.Fall);
        
        // if jump was pressed -> we are jumping, i.e switch states
        else if (player.JumpPressed)
            machine.ChangeState(player.Jump);


        // for now this bugs out, but I think it would be cute if the player just sat if you entered slide in idle. 
        //else if (player.Sliding) 
            //machine.ChangeState(player.Slide);

        // if move input is not 0 -> we are runinng, i.e switch states
        else if (player.MoveInput != 0)
            machine.ChangeState(player.Run);
    }

    public override void PhysicsUpdate()
    {
        // linear velocity is a vector2 (x, y) in units per second. 
        // set X to 0 so the player dosen't slide 
        // copy the CURRENT Y back in unchanged, so gravity keeps working normally.
        // you can't just set .x on its own; unity requires assinging a whole Vector 2
        player.Rb.linearVelocity = new Vector2(0f, player.Rb.linearVelocity.y);
    }

    public override void VariableAnimationChange()
    {

        player.FlipToFaceInput();
        if (currentAnim != "Idle0" && (player.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)) 
        // if the current animation is not default idle, check if it has finished
        {
            player.Anim.Play("Idle0");
            // if so switch back to idle animation and reset current animation value
            currentAnim = "Idle0";
            // determine random interval
            float randInterval = Random.Range(3f, 4f);
        }
        else
        {
            // if not, increment the idle timer
            idleTimer += Time.deltaTime;
            // if our interval is over, play a new random animation, reset the timer
            if (idleTimer >= randInterval)
            {
                string anim = IdleAnims[Random.Range(1, IdleAnims.Length)];
                player.Anim.Play(anim, 0, 0f);
                currentAnim = anim;
                idleTimer = 0f;
            }
        }
    }
}
