public class AttackState : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        // Attack Animation Start
        player.StopPlayer();
    }

    public void UpdateState(PlayerController player)
    {
        if (player.JoystickInputMagnitude() > 0.1f)
        {
            player.TransitionToState(new RunningState());
        }
    }

    public void FixedUpdateState(PlayerController player)
    {
        // AttackState'de fiziksel hareket yapılmaz.
    }

    public void ExitState(PlayerController player)
    {
        // Attack Animation End
    }
}