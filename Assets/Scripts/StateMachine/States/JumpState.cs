using UnityEngine;

public class JumpState : PlayerState
{
    public JumpState(PlayerController p, PlayerStateMachine m) : base(p, m) {}

    public override void Enter() 
    {
        player.ApplyJumpMovement();
        player.Anim.Play("JumpUp");
    }

    public override void LogicUpdate()
    {
        
        if (player.IsFalling()) 
            machine.ChangeState(player.Fall);
        
        // check linear velocity to determine if player is leaving the ground on a jump
        // if this passes, we know we are on the ground, and this condition is required for all others
        else if (player.IsGrounded() && player.Rb.linearVelocity.y <= .01f) 
        {
            // player.Sliding == true -> change state to slide
            if (player.Sliding) 
                machine.ChangeState(player.Slide); 

            // move input != 0 -> change state to run 
            if (player.MoveInput != 0) 
                machine.ChangeState(player.Run);

            if (player.MoveInput == 0 && player.Rb.linearVelocity.x != 0)
                machine.ChangeState(player.Skid);

            // if move input == 0 -> change state to idle
            else if (player.MoveInput == 0)
                machine.ChangeState(player.Idle);
        }
        
    }

    // physics update for the strafe movement
    public override void PhysicsUpdate()
    {
        player.ApplyStrafe();    
    }

    // this handles the variable animation changes for the jump state
    public override void VariableAnimationChange()
    {
        if (player.ChangeFallAnimation()) player.Anim.Play("JumpFlat");
        player.FlipToFaceInput();
    }

}
