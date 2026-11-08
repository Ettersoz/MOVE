

public class PlayerStateMachine 
{
    // PUBLIC GET SO OTHER SCRIPS (ui, debug text) can see what state we are in, 
    // only machine can set it

    public PlayerState CurrentState { get; private set; }

    // called once at startup to pick the first state
    public void Initialize(PlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    // every transtion goes through here, so you can never 
    // forget to run Exit on the old state 
    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
