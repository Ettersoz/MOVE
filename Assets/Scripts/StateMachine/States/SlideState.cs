using UnityEngine;
using UnityEngine.PlayerLoop;

public class SlideState : PlayerState
{
    float initialMoveInput = 0f;

    float maxSlideTimer = 0f;

    public override bool CanQueueInput => true;

    public SlideState(PlayerController p, PlayerStateMachine m) : base(p, m) {}

    public override void Enter()
    { 
        // store what direciton the player is going in when entering the slide state
        // if there is no direction being held, if so, use the flip to face input value 
        // this ensures the player gets momentum in the direction they are facing
        initialMoveInput = player.MoveInput != 0
            ? Mathf.Sign(player.MoveInput)
            : Mathf.Sign(player.transform.localScale.x);
        // play slide animation, update to be variable on the speed of the slide
        player.Anim.Play("Slide");  
        // set the initial slide velocity to the player's slide force
        player.currentSlideVelocity = player.slideForce + Mathf.Abs(player.Rb.linearVelocity.x);
        // set slide timer to 0 
        maxSlideTimer = 0f;
    } 

    public override void LogicUpdate()
    {
        maxSlideTimer += Time.deltaTime;

        // if the player is falling, change to the fall state
        if (!player.IsGrounded()) machine.ChangeState(player.Fall);

        // currently you can only jump out of a slide. may change this. 
        else if (player.IsGrounded())
        {
            if (player.JumpPressed) 
                machine.ChangeState(player.Jump);
            
            else if (player.skid)
                machine.ChangeState(player.Skid);
        
            // if the player is not moving from slide, make them stand up
            else if (player.Rb.linearVelocity.x == 0)
                machine.ChangeState(player.Idle);
        }
    }

    // this doesn't feel right to me 
    // what i want is not necissarily an upper cap,  
    // i might want this to be weighted heavier the faster 
    // the player is going, so the last couple of percetage points cover a higher range
    // maybe something like remove everything above the walk speed, divde it by 3, and add that onto the 
    // total clamp, it makes the animation speed far more variable, but I think it will be noticable for 
    // people actually going variable speeds. 

    public override void VariableAnimationChange()
    {
        // slide percent is the percentage speed player is moving compared to the slide force. 
        float speedPercent = Mathf.Abs(player.Rb.linearVelocity.x) / (player.slideForce);

        speedPercent = Mathf.Clamp(speedPercent, 0f, 2.5f);

        player.Anim.SetFloat("SlideSpeed", speedPercent);
    }

    // physics update for the slide movement
    public override void PhysicsUpdate()
    {
        player.ApplySlide(initialMoveInput, maxSlideTimer);
    }

}
