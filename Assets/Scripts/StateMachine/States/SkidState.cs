using UnityEngine;
using UnityEngine.InputSystem;

public class SkidState : PlayerState
{
    public SkidState(PlayerController p, PlayerStateMachine m) : base(p, m) {}

    public override void Enter()
    {
   
        player.Anim.Play("Skid");
    }

    public override void LogicUpdate()
    {
        if (!player.IsGrounded())
        {
            machine.ChangeState(player.Fall);
        }

        else if (player.JumpPressed)
        {
            machine.ChangeState(player.Jump);
        }

        else if (player.MoveInput != 0)
        {
            machine.ChangeState(player.Run);
        }

        else if (player.Rb.linearVelocity.x == 0)
        {
            machine.ChangeState(player.Idle);
        }

    }

    public override void PhysicsUpdate()
    {
        player.ApplySkid();
    }

}
