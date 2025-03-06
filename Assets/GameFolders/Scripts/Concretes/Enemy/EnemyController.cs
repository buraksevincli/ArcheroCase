using System.Collections.Generic;
using HHGArchero.Enemies;
using HHGArchero.Managers;
using UnityEngine;

namespace HHGArchero.Enemy
{
    public class EnemyController : MonoBehaviour, IDamageable
    {
        private EnemyPoolManager _spawnManager;
        private int _health = 100;
        private bool _isDead;

        private struct BurnEffect
        {
            public float NextTickTime;
            public int TicksRemaining; 
            public int DamagePerTick;  
        }
        
        private List<BurnEffect> _burnEffects = new List<BurnEffect>();

        private readonly float _burnTickInterval = 1f;
        
        private void Update()
        {
            if (GameManager.Instance.IsPaused || _isDead)
                return;

            for (int i = _burnEffects.Count - 1; i >= 0; i--)
            {
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
                    if (effect.TicksRemaining <= 0 )
                    {
                        _burnEffects.RemoveAt(i);
                    }
                    else
                    {
                        _burnEffects[i] = effect; 
                    }
                }
            }
        }

        private void Die()
        {
            _isDead = true;
            _spawnManager.ReturnAndRespawn(this);
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
            if (burnDuration <= 0)
                return;
            
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
