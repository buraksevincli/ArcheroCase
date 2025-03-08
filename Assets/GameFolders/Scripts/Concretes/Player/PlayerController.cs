using UnityEngine;
using HHGArchero.Projectile;
using HHGArchero.Enemy;
using HHGArchero.Extensions;
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
        private EnemyController _currentEnemy;
        private IPlayerState _currentState;
        private bool _canMove = true;

        public EnemyController CurrentEnemy
        {
            get => _currentEnemy;
            set => _currentEnemy = value;
        }

        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
            _mover = new Mover(rigidbody, joystick);
            
        }

        private void Start()
        {
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
            Transform target = EnemyPoolManager.Instance.ActiveEnemies.GetClosestTransform(bowTransform);
            if (!target) return;
            target.TryGetComponent(out EnemyController targetEnemy);
            _currentEnemy = targetEnemy;
            _currentEnemy.OnDeath += OnDeathEnemyHandler;
            _currentEnemy.SelectedTarget(true);
            transform.LookAt(target);

        }

        private void OnDeathEnemyHandler()
        {
            _currentEnemy.OnDeath -= OnDeathEnemyHandler;
            _currentEnemy = null;
        }

        public void UnselectTarget()
        {
            if(!_currentEnemy) return;
            _currentEnemy.SelectedTarget(false);
            OnDeathEnemyHandler();
        }

        public void FireSingleProjectile()
        {
            if(!_currentEnemy) SelectTarget();
            transform.LookAt(_currentEnemy.transform);
            Vector3 launchVelocity;
            if (!ProjectileHelper.CalculateLaunchVelocity(bowTransform.position, _currentEnemy.transform.position, out launchVelocity))
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
