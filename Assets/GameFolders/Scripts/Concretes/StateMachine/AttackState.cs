using HHGArchero.Managers;
using HHGArchero.Player;
using UnityEngine;

namespace HHGArchero.StateMachine
{
    public class AttackState : IPlayerState
    {
        private float _fireRate = DataManager.Instance.ProjectileData.FireRate;
        private float _projectileDelay = DataManager.Instance.ProjectileData.ProjectileDelay;

        private float _fireTimer = 0f;
        private int _projectileFired = 0;
        private float _lastProjectileTime = 0f;

        public void EnterState(PlayerController player)
        {
            player.SetAttackAnimation(true);
            player.StopPlayer();
            _fireTimer = 0f;
            _projectileFired = 0;
            _lastProjectileTime = Time.time;
        }

        public void UpdateState(PlayerController player)
        {
            if (player.JoystickInputMagnitude() > 0.1f)
            {
                player.TransitionToState(new RunningState());
                return;
            }

            _fireTimer += Time.deltaTime;
            int arrowCount = SkillManager.Instance.ProjectileMultiplicationCount;

            if (_projectileFired == 0 && _fireTimer >= _fireRate / SkillManager.Instance.ProjectileFireSpeedCount)
            {
                player.FireSingleProjectile();
                _projectileFired = 1;
                _lastProjectileTime = Time.time;
            }
            else if (_projectileFired > 0 && _projectileFired < arrowCount)
            {
                if (Time.time - _lastProjectileTime >= _projectileDelay)
                {
                    player.FireSingleProjectile();
                    _projectileFired++;
                    _lastProjectileTime = Time.time;
                }
            }
            else if (_projectileFired == arrowCount)
            {
                _fireTimer = 0f;
                _projectileFired = 0;
            }
        }

        public void FixedUpdateState(PlayerController player)
        {
            //
        }

        public void ExitState(PlayerController player)
        {
            player.SetAttackAnimation(false);
            _fireTimer = 0f;
            _projectileFired = 0;
        }
    }
}
