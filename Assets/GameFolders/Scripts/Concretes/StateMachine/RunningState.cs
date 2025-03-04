using HHGArchero.Player;

namespace HHGArchero.StateMachine
{
    public class RunningState : IPlayerState
    {
        public void EnterState(PlayerController player)
        {
            player.SetRunningAnimation(true);
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
            player.SetRunningAnimation(false);
        }
    }
}