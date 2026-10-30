using UnityEngine;

public class JumpState : PlayerState
{
    public JumpState(PlayerController p, PlayerStateMachine m) : base(p, m) {}

    public override void Enter() => player.Anim.Play("Jump");

    public override void LogicUpdate()
    {
        if (!player.IsGrounded())
            machine.ChangeState(player.Fall);
        
        else if (player.MoveInput != 0) 
            machine.ChangeState(player.Run);

        else if (player.MoveInput == 0)
            machine.ChangeState(player.Idle);
    
    }

    // need jump logic in player controller to get jump working
    // maybe "apply vertical movement" seems a little dump though
}
