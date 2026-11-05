using Unity.Mathematics;
using UnityEngine;

public class FallState : PlayerState
{
    public FallState(PlayerController p, PlayerStateMachine m) : base(p, m) {}

    //public override void Enter() => player.Anim.Play("Fall");

    public override void LogicUpdate()
    {
        if(player.IsGrounded())
        {
            
            if (player.Sliding) 
                machine.ChangeState(player.Slide);

            // if there is player input, run state
            if (player.MoveInput != 0)
                machine.ChangeState(player.Run);
            
            // if jump is being pressed, jump (:
            else if (player.JumpPressed) 
                machine.ChangeState(player.Jump);

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
    }

    public override void VariableAnimationChange()
    {
        if (player.ChangeFallAnimation()) player.Anim.Play("Fall");
    }

}
