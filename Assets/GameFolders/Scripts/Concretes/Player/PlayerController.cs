using HHGArchero.StateMachine;
using UnityEngine;

#pragma warning disable CS0108, CS0114

namespace HHGArchero.Player
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private DynamicJoystick joystick;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;

        private Mover _mover;
        private IPlayerState _currentState;

        private void Awake()
        {
            _mover = new Mover(rigidbody, joystick, moveSpeed);
            _currentState = new AttackState();
            _currentState.EnterState(this);
        }

        private void Update()
        {
            _currentState.UpdateState(this);
        }

        private void FixedUpdate()
        {
            _currentState.FixedUpdateState(this);
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
            // Set Attack Animation
        }

        public void SetRunningAnimation(bool isRunning)
        {
            // Set Running Animation
        }
    }
}