using UnityEngine;

public class SkidState : PlayerState
{
    public SkidState(PlayerController p, PlayerStateMachine m) : base(p, m) {}

    public override void Enter()
    {
        player.Anim.Play("Skid");
    }
}
