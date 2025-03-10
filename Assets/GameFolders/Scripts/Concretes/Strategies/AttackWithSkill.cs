using HHGArchero.Managers;
using HHGArchero.StateMachine;
using UnityEngine;

namespace HHGArchero.Strategies
{
    public class AttackWithSkill : IAttackStrategy
    {
        private int _arrowCount;
        public void OnEnter(AttackContext context)
        {
            context.FireTimer = 0f;
            context.FireRate /= 2f;
            context.ProjectilesFired = 0;
            context.LastProjectileTime = Time.time;
            context.Player.StopPlayer();
            context.Player.SelectTarget();
            context.Player.SetAttackAnimation(true);
        }

        public void OnUpdate(AttackContext context)
        {
            if (context.Player.JoystickInputMagnitude() > 0.1f)
            {
                context.Player.TransitionToState(new RunningState());
                return;
            }
            context.FireTimer += Time.deltaTime;
            FireProjectile(context);
        }
        
        private void FireProjectile(AttackContext context)
        {
            _arrowCount = SkillManager.Instance.ProjectileMultiplicationCount;

            if (context.ProjectilesFired == 0 && context.FireTimer >= context.FireRate / SkillManager.Instance.ProjectileFireSpeedCount)
            {
                context.FireRate = DataManager.Instance.ProjectileData.FireRate;
                context.Player.SetAnimationSpeed(SkillManager.Instance.ProjectileFireSpeedCount / context.FireRate);
                context.Player.FireSingleProjectile();
                context.ProjectilesFired = 1;
                context.LastProjectileTime = Time.time;
            }
            else if (context.ProjectilesFired > 0 && context.ProjectilesFired < _arrowCount)
            {
                if (Time.time - context.LastProjectileTime >= context.ProjectileDelay)
                {
                    context.Player.FireSingleProjectile();
                    context.ProjectilesFired++;
                    context.LastProjectileTime = Time.time;
                }
            }
            else if (context.ProjectilesFired == _arrowCount)
            {
                ResetFireCycle(context);
            }
        }
        
        private void ResetFireCycle(AttackContext context)
        {
            context.FireTimer = 0f;
            context.ProjectilesFired = 0;
        }

        public void OnExit(AttackContext context)
        {
            context.Player.SetAttackAnimation(false);
            context.Player.UnselectTarget();
            context.Player.SetAnimationSpeed(1);
            context.FireRate = DataManager.Instance.ProjectileData.FireRate;
        }
    }
}