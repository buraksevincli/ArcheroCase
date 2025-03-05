using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HHGArchero.Projectile;
using HHGArchero.Enemy;
using HHGArchero.StateMachine;
using HHGArchero.Utilities;
#pragma warning disable CS0108, CS0114

namespace HHGArchero.Player
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class PlayerController : MonoSingleton<PlayerController>
    {
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private DynamicJoystick joystick;
        [SerializeField] private Transform bowTransform;
        [SerializeField] private ProjectilePoolManager projectilePoolManager;
        
        private Mover _mover;
        private IPlayerState _currentState;
        
        protected override void Awake()
        {
            base.Awake();
            _mover = new Mover(rigidbody, joystick);
            _currentState = new AttackState();
            _currentState.EnterState(this);
        }
        
        private void Update()
        {
            _currentState.UpdateState(this);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireProjectile();
            }
        }
        
        private void FixedUpdate()
        {
            _currentState.FixedUpdateState(this);
        }
        
        private void FireProjectile()
        {
            List<Transform> activeEnemies = EnemyPoolManager.Instance.ActiveEnemies;
            if (activeEnemies == null || activeEnemies.Count == 0)
                return;
    
            Transform target = activeEnemies
                .OrderBy(e => Vector3.Distance(bowTransform.position, e.position))
                .FirstOrDefault();
    
            if (!target)
                return;
            transform.LookAt(target);
            
            Vector3 launchVelocity;
            if (!ProjectileHelper.CalculateLaunchVelocity(bowTransform.position, target.position, out launchVelocity))
            {
                return;
            }
            
            ProjectileController projectile = projectilePoolManager.GetProjectile();
            projectile.SetPoolManager(projectilePoolManager);
            projectile.transform.position = bowTransform.position;
            projectile.Launch(launchVelocity);
        }
        
        public void TransitionToState(IPlayerState newState)
        {
            _currentState.ExitState(this);
            _currentState = newState;
            _currentState.EnterState(this);
        }
        
        public float JoystickInputMagnitude()
        {
            return new Vector2(joystick.Horizontal, joystick.Vertical).magnitude;
        }
        
        public void MovePlayer()
        {
            _mover.Move(transform);
        }
        
        public void StopPlayer()
        {
            _mover.Stop();
        }
        
        public void SetAttackAnimation(bool isAttacking)
        {
            // Set Attack Animation here
        }
        
        public void SetRunningAnimation(bool isRunning)
        {
            // Set Running Animation here
        }
    }
}
