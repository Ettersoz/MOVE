using NUnit.Framework;
using NUnit.Framework.Constraints;
using Unity.Mathematics;
using UnityEngine;

public class FallState : PlayerState
{
    public FallState(PlayerController p, PlayerStateMachine m) : base(p, m) {}

    public override void Enter() => player.Anim.Play("Fall");

    public override void LogicUpdate()
    {
        if(player.IsGrounded())
        {
            if (player.MoveInput != 0)
                machine.ChangeState(player.Run);
            
            else if (player.JumpPressed) 
                machine.ChangeState(player.Jump);

            else if (player.MoveInput == 1)
                machine.ChangeState(player.Idle);
        }
    }

}
