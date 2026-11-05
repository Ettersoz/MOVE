using System;
using UnityEngine;



public class RunState : PlayerState 
{
    // call the constructor for an run state 
    public RunState(PlayerController m, PlayerStateMachine p) : base(m, p) {}

    private float slideTimer = 0f; 

    public override void Enter()
    {
        Debug.Log("Entering Run State");        
        player.Anim.Play("Run"); 
        slideTimer = 0f; 
    } 

    public override void LogicUpdate()
    {

        slideTimer += Time.deltaTime;

        // we want to override so we are seeing the right things as they happen 
        if (!player.IsGrounded()) // if the player is not grounded, unilaterally we change the state to fall
            machine.ChangeState(player.Fall);

        else if (player.JumpPressed) // if jump pressed -> change the state to jump
            machine.ChangeState(player.Jump);

        else if (player.Sliding && Mathf.Abs(player.Rb.linearVelocity.x) > 3 && slideTimer > 0.5f) // if slide is active -> change state to slide         
            machine.ChangeState(player.Slide);  
        
        else if (player.MoveInput == 0) // if no move input -> change state to idle
            machine.ChangeState(player.Idle);
    }

    // we need to also actually apply the horizontal movement so 
    public override void PhysicsUpdate() {
        player.ApplyHorizontalMovement();
    }
}
