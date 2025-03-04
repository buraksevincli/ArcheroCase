using UnityEngine;

namespace HHGArchero.Player
{
    public class Mover
    {
        private readonly Rigidbody _rigidbody;
        private readonly DynamicJoystick _joystick;
        private readonly float _moveSpeed;

        public Mover(Rigidbody rigidbody, DynamicJoystick joystick, float moveSpeed)
        {
            _rigidbody = rigidbody;
            _joystick = joystick;
            _moveSpeed = moveSpeed;
        }

        public void Move(Transform transform)
        {
            _rigidbody.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, _rigidbody.velocity.y, _joystick.Vertical * _moveSpeed);

            if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
            {
                transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
            }
        }

        public void Stop()
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }
}