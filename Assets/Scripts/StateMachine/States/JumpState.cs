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
        {
            machine.ChangeState(player.Fall);
        }
        
        else if (player.IsGrounded() && player.Rb.linearVelocity.y <= .01f) 
        {
            if (player.Sliding) machine.ChangeState(player.Slide);

            if (player.MoveInput != 0) 
                machine.ChangeState(player.Run);

            else if (player.MoveInput == 0)
                machine.ChangeState(player.Idle);
        }
        
    }

    public override void PhysicsUpdate()
    {
        player.ApplyStrafe();    
    }

    public override void VariableAnimationChange()
    {
        if (player.ChangeFallAnimation()) player.Anim.Play("JumpFlat");
    }

    // need jump logic in player controller to get jump working
    // maybe "apply vertical movement" seems a little dump though
}
