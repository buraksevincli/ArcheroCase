using System.Collections;
using HHGArchero.Enemies;
using HHGArchero.Enemy;
using HHGArchero.Extensions;
using HHGArchero.Managers;
using UnityEngine;

namespace HHGArchero.Projectile
{
    public class ProjectileController : MonoBehaviour
    {
        [Header("Particle & Visuals")] [SerializeField]
        private ParticleSystem burnEffect;

        private ProjectilePoolManager _poolManager;
        private Coroutine _movementCoroutine;
        private Vector3 _initialVelocity;
        private Vector3 _startPos;

        private float _timeSinceLaunch;
        private bool _launched;
        private bool _canMove = true;

        private int _remainingBounces;

        private void OnEnable() => GameManager.Instance.OnGamePaused += HandlePause;
        private void OnDisable() => GameManager.Instance.OnGamePaused -= HandlePause;
        public void SetPoolManager(ProjectilePoolManager poolManager) => _poolManager = poolManager;
        private void HandlePause(bool isPaused) => _canMove = !isPaused;

        public void Launch(Vector3 initialVelocity)
        {
            gameObject.SetActive(true);
            _launched = true;
            _initialVelocity = initialVelocity;
            _startPos = transform.position;
            _remainingBounces = SkillManager.Instance.ProjectileBounceCount;

            ResetCoroutine(ref _movementCoroutine, ProjectileMotion());
        }

        private void ResetCoroutine(ref Coroutine coroutine, IEnumerator routine)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(routine);
        }

        private IEnumerator ProjectileMotion()
        {
            HandleBurnEffect();
            float time = 0f;

            int projectileLifetime = DataManager.Instance.ProjectileData.ProjectileLifeTime;
            _timeSinceLaunch = 0f;

            while (_launched)
            {
                if (!_canMove)
                {
                    yield return null; // Pauses the coroutine's execution and resumes it in the next frame.
                    continue;
                }

                time += Time.deltaTime;
                if (time >= projectileLifetime)
                {
                    time = 0;
                    ReturnToPool();
                    yield break;
                }

                // Projectile motion: position = start + v0*t + 0.5*g*t^2
                _timeSinceLaunch += Time.deltaTime * 2f;
                transform.position = _startPos
                                     + _initialVelocity * _timeSinceLaunch
                                     + Physics.gravity * (0.5f * _timeSinceLaunch * _timeSinceLaunch);

                Vector3 velocity = _initialVelocity + Physics.gravity * _timeSinceLaunch;
                if (velocity.sqrMagnitude > 0.01f)
                    transform.rotation = Quaternion.LookRotation(velocity);

                yield return null; // Pauses the coroutine's execution and resumes it in the next frame.
            }
        }

        private void HandleBurnEffect()
        {
            if (SkillManager.Instance.ProjectileBurnTime > 0)
                burnEffect.Play();
            else
                burnEffect.Stop();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_launched) return;

            if (!other.TryGetComponent(out IDamageable damageable)) return;

            damageable.TakeDamage(DataManager.Instance.ProjectileData.ProjectileDamage);
            TryApplyBurn(other);
            if (_remainingBounces > 0)
                BounceToNextEnemy(other.transform);
            else
                ReturnToPool();
        }

        private void TryApplyBurn(Collider other)
        {
            if (SkillManager.Instance.ProjectileBurnTime <= 0) return;

            if (other.TryGetComponent(out EnemyController enemy))
            {
                int burnDamage = DataManager.Instance.ProjectileData.ProjectileBurnDamage;
                float burnDuration = SkillManager.Instance.ProjectileBurnTime;
                enemy.ApplyBurn(burnDamage, burnDuration);
            }
        }

        private void BounceToNextEnemy(Transform currentEnemy)
        {
            transform.position = currentEnemy.position;
            _startPos = currentEnemy.position;

            Transform nextTarget = EnemyPoolManager.Instance.ActiveEnemies
                .GetClosestTransform(currentEnemy, currentEnemy);

            if (nextTarget != null &&
                ProjectileHelper.CalculateLaunchVelocity(transform.position,
                    nextTarget.position,
                    out Vector3 newLaunchVelocity))
            {
                _initialVelocity = newLaunchVelocity;
                _timeSinceLaunch = 0f;
                _remainingBounces--;
                return;
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