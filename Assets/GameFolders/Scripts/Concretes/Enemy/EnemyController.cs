using System;
using System.Collections.Generic;
using HHGArchero.Enemies;
using HHGArchero.Managers;
using HHGArchero.Scriptables;
using HHGArchero.UI;
using UnityEngine;

namespace HHGArchero.Enemy
{
    public class EnemyController : MonoBehaviour, IDamageable
    {
        [SerializeField] private HealthBarManager healthBarManager;
        [SerializeField] private GameObject selectedEffect;
        [SerializeField] private ParticleSystem burnEffect;

        private EnemyPoolManager _spawnManager;
        private EnemyData _enemyData;
        private ProjectileData _projectileData;

        private int _health;
        private float _burnTickInterval;
        private bool _isSelected;
        private bool _isPaused;
        private bool _isBurning;
        private bool _isDead;

        private List<BurnEffect> _burnEffects = new List<BurnEffect>();

        public event Action OnDeath;

        private struct BurnEffect
        {
            public float NextTickTime;
            public int TicksRemaining;
            public int DamagePerTick;
        }

        private void Awake() => InitializeData();
        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnsubscribeEvents();
        
        private void Update()
        {
            if (_health <= 0 && !_isDead) Die();
            UpdateUI();
            if (_isPaused || _isDead) return;
            UpdateBurnEffects();
        }

        private void InitializeData()
        {
            _enemyData = DataManager.Instance.EnemyData;
            _projectileData = DataManager.Instance.ProjectileData;

            _health = _enemyData.MaxHealth;
            _burnTickInterval = _projectileData.ProjectileBurnTickRate;
            _isDead = false;
            _isPaused = false;
            _isBurning = false;
        }

        public void SetSpawnManager(EnemyPoolManager spawnManager)
        {
            _spawnManager = spawnManager;
            ResetEnemy();
        }

        private void ResetEnemy()
        {
            _health = _enemyData.MaxHealth;
            _isDead = false;
            _isBurning = false;
            _isSelected = false;
            _burnEffects.Clear();
        }

        private void SubscribeEvents() => GameManager.Instance.OnGamePaused += OnGamePausedHandler;
        private void UnsubscribeEvents() => GameManager.Instance.OnGamePaused -= OnGamePausedHandler;
        public void TakeDamage(int damage) => _health -= damage;

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

        private void UpdateBurnEffects()
        {
            if (_burnEffects.Count > 0 && !_isBurning)
            {
                _isBurning = true;
                burnEffect.Play();
            }

            for (int i = _burnEffects.Count - 1; i >= 0; i--)
            {
                BurnEffect effect = _burnEffects[i];

                if (Time.time >= effect.NextTickTime)
                {
                    TakeDamage(effect.DamagePerTick);
                    if (_isDead || _burnEffects.Count == 0) return;

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

        private void OnGamePausedHandler(bool isPaused)
        {
            _isPaused = isPaused;
            if (_isBurning)
            {
                if (_isPaused) burnEffect.Pause();
                else burnEffect.Play();
            }
        }

        private void Die()
        {
            OnDeath?.Invoke();
            _burnEffects.Clear();
            _isBurning = false;
            _isDead = true;
            _isSelected = false;
            _health = _enemyData.MaxHealth;
            _spawnManager.ReturnAndRespawn(this);
        }

        public void SelectedTarget(bool isSelected) => _isSelected = isSelected;

        private void UpdateUI()
        {
            healthBarManager.UpdateHealthBar(_enemyData.MaxHealth, _health);
            selectedEffect.SetActive(_isSelected);
        }
    }
}