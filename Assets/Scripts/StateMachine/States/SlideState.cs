using UnityEngine;

public class SlideState : PlayerState
{
    public SlideState(PlayerController p, PlayerStateMachine m) : base(p, m) {}

    public override void Enter() => player.Anim.Play("Slide");

    public override void LogicUpdate()
    {
        if (player.IsFalling()) machine.ChangeState(player.Fall);

        else if (player.IsGrounded())
        {
            if (player.IsFalling())
                machine.ChangeState(player.Fall);

            else if (player.JumpPressed) 
                machine.ChangeState(player.Jump);
        }
    }
}
