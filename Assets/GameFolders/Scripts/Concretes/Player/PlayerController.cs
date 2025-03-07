using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HHGArchero.Projectile;
using HHGArchero.Enemy;
using HHGArchero.Managers;
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
        
        private Animator _animator;
        private Mover _mover;
        private Transform _currentEnemy;
        private IPlayerState _currentState;
        private bool _canMove = true;
        
        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
            _mover = new Mover(rigidbody, joystick);
            _currentState = new AttackState();
            _currentState.EnterState(this);
        }

        private void OnEnable()
        {
            GameManager.Instance.OnGamePaused += OnGamePausedHandler;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnGamePaused -= OnGamePausedHandler;
        }

        
        private void Update()
        {
            if (!_canMove) return;
            _currentState.UpdateState(this);
        }
        
        private void FixedUpdate()
        {
            if (!_canMove) return;
            _currentState.FixedUpdateState(this);
        }

        public void SelectTarget()
        {
            List<Transform> activeEnemies = EnemyPoolManager.Instance.ActiveEnemies;
            if (activeEnemies == null || activeEnemies.Count == 0)
                return;
    
            Transform target = activeEnemies
                .OrderBy(e => Vector3.Distance(bowTransform.position, e.position))
                .FirstOrDefault();
            if (!target) return;
            _currentEnemy = target;
            transform.LookAt(target);
        }

        public void FireSingleProjectile()
        {
            SelectTarget();
            transform.LookAt(_currentEnemy);
            Vector3 launchVelocity;
            if (!ProjectileHelper.CalculateLaunchVelocity(bowTransform.position, _currentEnemy.position, out launchVelocity))
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
        
        private void OnGamePausedHandler(bool isPaused)
        {
            _canMove = !isPaused;
            _animator.enabled = !isPaused;
        }
        
        public void SetAttackAnimation(bool isAttacking)
        {
            _animator.SetBool("IsAttack", isAttacking);
        }
        
        public void SetRunningAnimation(bool isRunning)
        {
            _animator.SetBool("IsRun", isRunning);
        }

        public void SetAnimationSpeed(float animationSpeed)
        {
            _animator.speed = animationSpeed;
        }
    }
}
