using HHGArchero.Managers;
using HHGArchero.Player;
using UnityEngine;

namespace HHGArchero.StateMachine
{
    public class AttackState : IPlayerState
    {
        private float _fireRate = DataManager.Instance.GameData.FireRate;
        private float _fireTimer = 0f;

        public void EnterState(PlayerController player)
        {
            player.SetAttackAnimation(true);
            player.StopPlayer();
            _fireTimer = DataManager.Instance.GameData.FireRate / 2;
        }

        public void UpdateState(PlayerController player)
        {
            _fireTimer += Time.deltaTime;
            if (_fireTimer >= _fireRate)
            {
                player.FireProjectile();
                _fireTimer = 0f;
            }

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
}