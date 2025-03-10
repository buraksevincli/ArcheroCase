using HHGArchero.Managers;
using HHGArchero.StateMachine;
using UnityEngine;

namespace HHGArchero.Strategies
{
    public class NormalAttackStrategy : IAttackStrategy
    {
        public void OnEnter(AttackContext context)
        {
            context.FireRate /= 2f;
            context.FireTimer = 0f;
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
            FireSingleProjectile(context);
        }

        private void FireSingleProjectile(AttackContext context)
        {
            if (context.FireTimer >= context.FireRate)
            {
                context.FireRate = DataManager.Instance.ProjectileData.FireRate;
                context.Player.SetAnimationSpeed(1f);
                context.Player.FireSingleProjectile();
                context.FireTimer = 0f;
            }
        }

        public void OnExit(AttackContext context)
        {
            context.Player.SetAttackAnimation(false);
            context.Player.UnselectTarget();
            context.FireRate = DataManager.Instance.ProjectileData.FireRate;
        }
    }
}
