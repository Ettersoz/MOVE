using UnityEngine;

public class RunState : PlayerState 
{
    // call the constructor for an run state 
    public RunState(PlayerController m, PlayerStateMachine p) : base(m, p) {}

    public override void Enter() => player.Anim.Play("Run");

    public override void LogicUpdate()
    {
        // we want to override so we are seeing the right things as they happen 
        if (!player.IsGrounded())
            machine.ChangeState(player.Fall);

        if (player.JumpPressed) 
            machine.ChangeState(player.Jump);
        
        if (player.MoveInput == 0)
            machine.ChangeState(player.Idle);
    }

    // we need to also actually apply the horizontal movement so 
    public override void PhysicsUpdate() => player.ApplyHorizontalMovement();
}
