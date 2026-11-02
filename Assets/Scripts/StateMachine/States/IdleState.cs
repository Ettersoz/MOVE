using UnityEngine;


// class inherits from the PlayerState class
public class IdleState : PlayerState
{

    // this calls the constructor for the PlayerState class and initializes it with 
    // player controller p and PlayerStateMachine m,
    // the key word "base" refers to the base object "PlayerState" 
    public IdleState(PlayerController p, PlayerStateMachine m) : base(p, m) {}

    // the animator must be named exactly "Idle" 
    // the override keyword "overrides" the definition of the function in the superclass
    // PlayerState. This allows us to make specific changes to the class function 
    // base on the subclass we are making
    public override void Enter() => player.Anim.Play("Idle");

    public override void LogicUpdate()
    {
        // if not touching the ground -> we are falling, i.e switch states
        if (!player.IsGrounded())
            machine.ChangeState(player.Fall);
        
        // if jump was pressed -> we are jumping, i.e switch states
        else if (player.JumpPressed)
            machine.ChangeState(player.Jump);

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
}
