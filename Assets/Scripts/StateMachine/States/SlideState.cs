using UnityEngine;
using UnityEngine.PlayerLoop;

public class SlideState : PlayerState
{
    float initialMoveInput = 0;

    public SlideState(PlayerController p, PlayerStateMachine m) : base(p, m) {}

    public override void Enter()
    {
        // store what direciton the player is going in when entering the slide state
        initialMoveInput = player.MoveInput;
        // play slide animation, update to be variable on the speed of the slide
        player.Anim.Play("Slide");  
        // set the initial slide velocity to the player's slide force
        player.currentSlideVelocity = player.slideForce + Mathf.Abs(player.Rb.linearVelocity.x);
    } 

    public override void LogicUpdate()
    {
        // if the player is falling, change to the fall state
        if (player.IsFalling()) machine.ChangeState(player.Fall);

        // currently you can only jump out of a slide. may change this. 
        else if (player.IsGrounded())
        {
            if (player.JumpPressed) 
                machine.ChangeState(player.Jump);
            
            // if the player is not moving from slide, make them stand up
            else if (player.Rb.linearVelocity.x == 0)
                machine.ChangeState(player.Idle);
        }


    }

    // physics update for the slide movement
    public override void PhysicsUpdate()
    {
        player.ApplySlide(initialMoveInput);
    }

}
