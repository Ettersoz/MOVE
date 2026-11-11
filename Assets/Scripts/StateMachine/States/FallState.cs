using Unity.Mathematics;
using UnityEngine;

public class FallState : PlayerState
{
    public FallState(PlayerController p, PlayerStateMachine m) : base(p, m) {}

    public override bool CanQueueInput => true;

    //public override void Enter() => player.Anim.Play("Fall");

    public override void LogicUpdate()
    {
        if(player.IsGrounded())
        {
            
            PlayerState queued = player.useQueuedState();

            if (queued != null)
                machine.ChangeState(queued);
            // if jump is being pressed, jump (:
            else if (player.JumpPressed) 
                machine.ChangeState(player.Jump);
            else if (player.Sliding) 
                machine.ChangeState(player.Slide);
            // if there is player input, run state
            else if (player.MoveInput != 0)
                machine.ChangeState(player.Run);
            // else default to ilde
            else 
                machine.ChangeState(player.Idle);
        }
    }

    // update to make movement additive, requires new function 
    // in player controller
    public override void PhysicsUpdate()
    {
        player.ApplyStrafe();
        player.CustomJumpGravity();
    }

    public override void VariableAnimationChange()
    {
        if (player.ChangeFallAnimation()) player.Anim.Play("Fall");
        player.FlipToFaceInput();
    }

}
