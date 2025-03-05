using System.Collections;
using UnityEngine;

namespace HHGArchero.Projectile
{
    public class ProjectileController : MonoBehaviour
    {
        private ProjectilePoolManager _poolManager;
        private bool _launched = false;
        private Coroutine _movementCoroutine;
        private Vector3 _initialVelocity;
        private float _timeSinceLaunch = 0f;

        public void SetPoolManager(ProjectilePoolManager poolManager)
        {
            _poolManager = poolManager;
        }
        
        public void Launch(Vector3 initialVelocity)
        {
            gameObject.SetActive(true);
            _launched = true;
            _initialVelocity = initialVelocity;
            _timeSinceLaunch = 0f;
            if(_movementCoroutine != null)
                StopCoroutine(_movementCoroutine);
            _movementCoroutine = StartCoroutine(ProjectileMotion());
            StartCoroutine(ReturnAfterTimeout(3f));
        }
        
        private IEnumerator ProjectileMotion()
        {
            Vector3 startPos = transform.position;
            while (_launched)
            {
                _timeSinceLaunch += Time.deltaTime;
                // Projectile motion: position = start + v0*t + 0.5*g*t^2
                transform.position = startPos + _initialVelocity * _timeSinceLaunch + Physics.gravity * (0.5f * _timeSinceLaunch * _timeSinceLaunch);
                yield return null;
            }
        }

        private IEnumerator ReturnAfterTimeout(float timeout)
        {
            yield return new WaitForSeconds(timeout);
            if (_launched)
            {
                ReturnToPool();
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (_launched)
            {
                ReturnToPool();
            }
        }

        private void ReturnToPool()
        {
            _launched = false;
            if (_movementCoroutine != null)
            {
                StopCoroutine(_movementCoroutine);
                _movementCoroutine = null;
            }
            gameObject.SetActive(false);
            _poolManager?.ReturnProjectile(this);
        }
    }
}
