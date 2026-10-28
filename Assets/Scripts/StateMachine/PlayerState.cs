using Unity.Collections.Tests.CoreCLR.TestJobs;
using UnityEngine;

public abstract class PlayerState 
{
    
    protected PlayerController player;
    protected PlayerStateMachine machine;

    public PlayerState(PlayerController player, PlayerStateMachine machine)
    {
        this.player = player;
        this.machine = machine;
    }

    // runs once when we swtih into this state 
    // start animation or impulse, like jump 
    public virtual void Enter() {}

    // runs when we leave a state. clean up anything set up new enter 
    public virtual void Exit() {}

    // called from player controller every frame 
    // read input here and decide whether to transition to another state 
    public virtual void LogicUpdate() {}

    // called from PlayerController.FixedUpdate()
    // apply velocity/forces here so phyisics stays framrate-independent
    public virtual void PhysicsUpdate() {}
}
