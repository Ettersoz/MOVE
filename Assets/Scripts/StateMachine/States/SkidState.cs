using UnityEngine;
using UnityEngine.InputSystem;

public class SkidState : PlayerState
{
    public SkidState(PlayerController p, PlayerStateMachine m) : base(p, m) {}


    // checked to see if the player is moving fast enough to trigger the skid state
    public override void Enter()
    {
        bool skipped = player.SkipSkid();
        if (!skipped)
            player.Anim.Play("Skid");
    }

    public override void LogicUpdate()
    {
        // check if player is grounded, if not falling
        if (!player.IsGrounded())
            machine.ChangeState(player.Fall);
        // check if the player is jumping 
        else if (player.JumpPressed)
            machine.ChangeState(player.Jump);
        // if the player is running 
        else if (player.MoveInput != 0)
            machine.ChangeState(player.Run);
        // if the player is not running, idle
        else if (player.Rb.linearVelocity.x == 0)
            machine.ChangeState(player.Idle);
    }   

    public override void PhysicsUpdate()
    {
        player.ApplySkid();
    }

}
