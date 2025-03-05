using System;
using HHGArchero.Projectile;
using UnityEngine;

namespace HHGArchero.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        private EnemyPoolManager _spawnManager;
        public void SetSpawnManager(EnemyPoolManager spawnManager)
        {
            _spawnManager = spawnManager;
        }

        private void OnMouseDown()
        {
            if (_spawnManager != null)
            {
                _spawnManager.ReturnAndRespawn(this);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out ProjectileController projectile)) return;
            if (projectile == null) return;
            if (_spawnManager != null)
            {
                _spawnManager.ReturnAndRespawn(this);
            }
        }
    }
}