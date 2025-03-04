using HHGArchero.Player;

namespace HHGArchero.StateMachine
{
    public interface IPlayerState
    {
        public void EnterState(PlayerController player);
        public void UpdateState(PlayerController player);
        public void FixedUpdateState(PlayerController player);
        public void ExitState(PlayerController player);
    }
}