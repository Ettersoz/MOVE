using UnityEngine;
using UnityEngine.PlayerLoop;

public class SlideState : PlayerState
{
    float initialMoveInput = 0;

    public SlideState(PlayerController p, PlayerStateMachine m) : base(p, m) {}

    public override void Enter()
    {
        initialMoveInput = player.MoveInput;
        player.Anim.Play("Slide");  
        player.currentSlideVelocity = player.slideForce;

    } 

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

    public override void PhysicsUpdate()
    {
        player.ApplySlide(initialMoveInput);
    }

}
