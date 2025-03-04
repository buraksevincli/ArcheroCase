public class RunningState : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        // Start Running Animation
    }

    public void UpdateState(PlayerController player)
    {
        if (player.JoystickInputMagnitude() <= 0.1f)
        {
            player.TransitionToState(new AttackState());
        }
    }

    public void FixedUpdateState(PlayerController player)
    {
        player.MovePlayer();
    }

    public void ExitState(PlayerController player)
    {
        // End Running Animation
    }
}