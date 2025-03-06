using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HHGArchero.Enemies;
using HHGArchero.Enemy;
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
        private Vector3 _startPos;
        private int _remainingBounces;

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
            _startPos = transform.position;
            _remainingBounces = SkillManager.Instance.ProjectileBounceCount;
            if(_movementCoroutine != null)
                StopCoroutine(_movementCoroutine);
            _movementCoroutine = StartCoroutine(ProjectileMotion());
        }
        
        private IEnumerator ProjectileMotion()
        {
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
                transform.position = _startPos + _initialVelocity * _timeSinceLaunch + Physics.gravity * (0.5f * _timeSinceLaunch * _timeSinceLaunch); // Simulate gravity effect
                yield return null; // Pauses the coroutine's execution and resumes it in the next frame.
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (_launched)
            {
                if (other.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(DataManager.Instance.GameData.ProjectileDamage);
                }
                
                if (SkillManager.Instance.ProjectileBurnTime > 0)
                {
                    if (other.TryGetComponent(out EnemyController enemy))
                    {
                        int burnDamage = DataManager.Instance.GameData.ProjectileBurnDamage; 
                        float burnDuration = SkillManager.Instance.ProjectileBurnTime; 
                        enemy.ApplyBurn(burnDamage, burnDuration);
                    }
                }

                if (_remainingBounces > 0)
                {
                    BounceToNextEnemy(other.transform);
                }
                else
                {
                    ReturnToPool();
                }
            }
        }
        
        private void BounceToNextEnemy(Transform currentEnemy)
        {
            transform.position = currentEnemy.position;
            _startPos = currentEnemy.position; 

            List<Transform> activeEnemies = EnemyPoolManager.Instance.ActiveEnemies;
            Transform nextTarget = activeEnemies
                .Where(e => e != currentEnemy)
                .OrderBy(e => Vector3.Distance(currentEnemy.position, e.position))
                .FirstOrDefault();
    
            if (nextTarget != null)
            {
                Vector3 newLaunchVelocity;
                if (ProjectileHelper.CalculateLaunchVelocity(transform.position, nextTarget.position, out newLaunchVelocity))
                {
                    _initialVelocity = newLaunchVelocity;
                    _timeSinceLaunch = 0f;
                    _remainingBounces--;
                    return;
                }
            }
            ReturnToPool();
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
