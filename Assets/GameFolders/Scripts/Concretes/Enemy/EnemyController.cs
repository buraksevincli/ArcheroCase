using HHGArchero.Enemies;
using UnityEngine;

namespace HHGArchero.Enemy
{
    public class EnemyController : MonoBehaviour, IDamageable
    {
        private EnemyPoolManager _spawnManager;
        private int _health = 100;
        public void SetSpawnManager(EnemyPoolManager spawnManager)
        {
            _spawnManager = spawnManager;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_spawnManager == null || _health > 0) return;
            _health = 100;
            _spawnManager.ReturnAndRespawn(this);
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
        }
    }
}