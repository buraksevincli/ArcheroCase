public class AttackState : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        player.SetAttackAnimation(true);
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
        //
    }

    public void ExitState(PlayerController player)
    {
        player.SetAttackAnimation(false);
    }
}