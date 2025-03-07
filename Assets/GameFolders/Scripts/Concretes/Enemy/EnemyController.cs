using System;
using System.Collections.Generic;
using HHGArchero.Enemies;
using HHGArchero.Managers;
using HHGArchero.Scriptables;
using UnityEngine;

namespace HHGArchero.Enemy
{
    public class EnemyController : MonoBehaviour, IDamageable
    {
        [SerializeField] private GameObject selectedEffect;
        [SerializeField] private ParticleSystem burnEffect;
        private EnemyPoolManager _spawnManager;
        private EnemyData _enemyData;
        private ProjectileData _projectileData;
        private int _health;
        private bool _isDead;
        private float _burnTickInterval;
        private bool _isSelected;
        private bool _isPaused;
        private bool _isBurning;

        public event Action OnDeath;

        private struct BurnEffect
        {
            public float NextTickTime;
            public int TicksRemaining;
            public int DamagePerTick;
        }

        private List<BurnEffect> _burnEffects = new List<BurnEffect>();

        private void Awake()
        {
            _enemyData = DataManager.Instance.EnemyData;
            _projectileData = DataManager.Instance.ProjectileData;
            _health = _enemyData.MaxHealth;
            _burnTickInterval = _projectileData.ProjectileBurnTickRate;
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
            selectedEffect.SetActive(_isSelected);

            if (_isPaused || _isDead) return;

            for (int i = _burnEffects.Count - 1; i >= 0; i--)
            {
                if (!_isBurning)
                {
                    _isBurning = true;
                    burnEffect.Play();
                }
                BurnEffect effect = _burnEffects[i];
                if (Time.time >= effect.NextTickTime)
                {
                    TakeDamage(effect.DamagePerTick);
                    if (_isDead || _burnEffects.Count == 0)
                    {
                        return;
                    }

                    effect.NextTickTime += _burnTickInterval;
                    effect.TicksRemaining--;
                    if (effect.TicksRemaining <= 0)
                    {
                        _burnEffects.RemoveAt(i);
                    }
                    else
                    {
                        _burnEffects[i] = effect;
                    }
                }
            }

            if (_burnEffects.Count == 0 && _isBurning)
            {
                burnEffect.Stop();
                _isBurning = false;
            }
        }

        private void Die()
        {
            _isDead = true;
            OnDeath?.Invoke();
            _isSelected = false;
            burnEffect.Stop();
            _isBurning = false;
            _spawnManager.ReturnAndRespawn(this);
        }

        private void OnGamePausedHandler(bool isPaused)
        {
            _isPaused = isPaused;
            if (_isBurning)
            {
                if (_isPaused)
                {
                    burnEffect.Pause();
                }
                else
                {
                    burnEffect.Play();
                }
            }
        }

        public void SelectedTarget(bool isSelected)
        {
            _isSelected = isSelected;
        }

        public void SetSpawnManager(EnemyPoolManager spawnManager)
        {
            _spawnManager = spawnManager;
            _isDead = false;
            _burnEffects.Clear();
            _health = 100;
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                Die();
            }
        }

        public void ApplyBurn(int damagePerTick, float burnDuration)
        {
            if (burnDuration <= 0) return;

            int totalTicks = Mathf.RoundToInt(burnDuration / _burnTickInterval);

            BurnEffect newEffect = new BurnEffect
            {
                NextTickTime = Mathf.Ceil(Time.time),
                TicksRemaining = totalTicks,
                DamagePerTick = damagePerTick
            };
            _burnEffects.Add(newEffect);
        }
    }
}