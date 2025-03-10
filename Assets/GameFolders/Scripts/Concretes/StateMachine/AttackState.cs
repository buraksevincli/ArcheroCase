using HHGArchero.Managers;
using HHGArchero.Player;
using UnityEngine;

namespace HHGArchero.StateMachine
{
    public class AttackState : IPlayerState
    {
        private float _fireRate = DataManager.Instance.ProjectileData.FireRate / 2;
        private float _projectileDelay = DataManager.Instance.ProjectileData.ProjectileDelay;

        private float _fireTimer = 0f;
        private int _projectileFired = 0;
        private float _lastProjectileTime = 0f;

        public void EnterState(PlayerController player)
        {
            player.StopPlayer();
            _fireTimer = 0f;
            _projectileFired = 0;
            _lastProjectileTime = Time.time;
            player.SelectTarget();
            player.SetAttackAnimation(true);
        }

        public void UpdateState(PlayerController player)
        {
            if (player.JoystickInputMagnitude() > 0.1f)
            {
                player.TransitionToState(new RunningState());
                return;
            }

            _fireTimer += Time.deltaTime;
            FireProjectile(player);
        }

        private void FireProjectile(PlayerController player)
        {
            int arrowCount = SkillManager.Instance.ProjectileMultiplicationCount;

            if (_projectileFired == 0 && _fireTimer >= _fireRate / SkillManager.Instance.ProjectileFireSpeedCount)
            {
                _fireRate = DataManager.Instance.ProjectileData.FireRate;
                player.SetAnimationSpeed(SkillManager.Instance.ProjectileFireSpeedCount / _fireRate);
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
                ResetFireCycle();
            }
        }
        
        private void ResetFireCycle()
        {
            _fireTimer = 0f;
            _projectileFired = 0;
        }
        
        public void FixedUpdateState(PlayerController player){}

        public void ExitState(PlayerController player)
        {
            player.SetAttackAnimation(false);
            player.UnselectTarget();
            player.SetAnimationSpeed(1);
            _fireTimer = 0f;
            _projectileFired = 0;
        }
    }
}
