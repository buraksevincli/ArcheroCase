using System;
using System.Collections;
using HHGArchero.Enemies;
using HHGArchero.Managers;
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
        private bool _canMove = true;
        private int _projectileDamage = 10;

        private void OnEnable()
        {
            GameManager.Instance.OnGamePaused += OnGamePausedHandler;
        }
        
        private void OnDisable()
        {
            GameManager.Instance.OnGamePaused -= OnGamePausedHandler;
        }
        
        private void OnGamePausedHandler(bool isPaused)
        {
            _canMove = !isPaused;
        }

        public void SetPoolManager(ProjectilePoolManager poolManager)
        {
            _poolManager = poolManager;
        }
        
        public void Launch(Vector3 initialVelocity)
        {
            gameObject.SetActive(true);
            _launched = true;
            _initialVelocity = initialVelocity;
            if(_movementCoroutine != null)
                StopCoroutine(_movementCoroutine);
            _movementCoroutine = StartCoroutine(ProjectileMotion());
        }
        
        private IEnumerator ProjectileMotion()
        {
            Vector3 startPos = transform.position;
            float time = 0f;
            int projectileLifeTime = DataManager.Instance.GameData.ProjectileLifeTime;
            _timeSinceLaunch = 0f;
            while (_launched)
            {
                if (!_canMove)
                {
                    yield return null; // Pauses the coroutine's execution and resumes it in the next frame.
                    continue;
                }
                time += Time.deltaTime;
                if (time >= projectileLifeTime)
                {
                    time = 0;
                    ReturnToPool();
                    yield break;
                }
                _timeSinceLaunch += Time.deltaTime;
                // Projectile motion: position = start + v0*t + 0.5*g*t^2
                transform.position = startPos + _initialVelocity * _timeSinceLaunch + Physics.gravity * (0.5f * _timeSinceLaunch * _timeSinceLaunch); // Simulate gravity effect
                yield return null; // Pauses the coroutine's execution and resumes it in the next frame.
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (_launched)
            {
                if (other.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(_projectileDamage);
                }
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
