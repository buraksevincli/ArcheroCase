using System.Collections.Generic;
using System.Linq;
using HHGArchero.Managers;
using HHGArchero.ObjectPool;
using HHGArchero.Player;
using HHGArchero.Scriptables;
using HHGArchero.Utilities;
using UnityEngine;

namespace HHGArchero.Enemy
{
    public class EnemyPoolManager : MonoSingleton<EnemyPoolManager>
    {
        private EnemyController _pooledObject;
        private Transform _playerTransform;
        private EnemyData _enemyData;
        private Vector2 _spawnAreaMin, _spawnAreaMax;
        private int _enemyPoolSize;
        private float _minimumDistanceBetweenEnemies, _minimumDistanceFromPlayer;
        private ObjectPool<EnemyController> _objectPool;

        private List<Transform> _activeEnemies = new List<Transform>();
        public List<Transform> ActiveEnemies => _activeEnemies;

        protected override void Awake()
        {
            base.Awake();
            // Data Initialization
            _enemyData = DataManager.Instance.EnemyData;
            _pooledObject = _enemyData.PooledObject;
            _enemyPoolSize = _enemyData.EnemyPoolSize;
            _spawnAreaMin = _enemyData.SpawnAreaMin;
            _spawnAreaMax = _enemyData.SpawnAreaMax;
            _minimumDistanceBetweenEnemies = _enemyData.MinimumDistanceBetweenEnemies;
            _minimumDistanceFromPlayer = _enemyData.MinimumDistanceFromPlayer;
            
            // Pool check: Ensure the prefab has an EnemyController component.
            EnemyController prefabComponent = _pooledObject.GetComponent<EnemyController>();
            if (prefabComponent == null)
            {
                Debug.LogError("The pooled prefab must have an EnemyController component.", this);
                return;
            }

            _objectPool = new ObjectPool<EnemyController>(prefabComponent, _enemyPoolSize, transform);
        }

        private void Start()
        {
            _playerTransform = PlayerController.Instance.transform;
            
            for (int i = 0; i < _enemyPoolSize; i++)
            {
                SpawnAtRandomPosition();
            }
        }

        public void ReturnAndRespawn(EnemyController obj)
        {
            // Remove the enemy from the active list before returning it to the pool.
            if (_activeEnemies.Contains(obj.transform))
                _activeEnemies.Remove(obj.transform);

            _objectPool.ReturnToPool(obj);
            SpawnAtRandomPosition();
        }

        private void SpawnAtRandomPosition()
        {
            EnemyController pooledObj = _objectPool.Get();
            pooledObj.transform.position = GetValidRandomPosition();
            pooledObj.SetSpawnManager(this);
            // Add the enemy to the active list if not already added.
            if (!_activeEnemies.Contains(pooledObj.transform))
                _activeEnemies.Add(pooledObj.transform);
        }

        private Vector3 GetRandomPosition()
        {
            float randomX = Random.Range(_spawnAreaMin.x, _spawnAreaMax.x);
            float randomZ = Random.Range(_spawnAreaMin.y, _spawnAreaMax.y);
            return new Vector3(randomX, 1f, randomZ);
        }

        private bool IsPositionValid(Vector3 candidate)
        {
            if (transform.Cast<Transform>()
                .Where(child => child.gameObject.activeInHierarchy)
                .Any(child => Vector3.Distance(child.position, candidate) < _minimumDistanceBetweenEnemies))
            {
                return false;
            }
            return !_playerTransform || !(Vector3.Distance(_playerTransform.position, candidate) < _minimumDistanceFromPlayer);
        }

        private Vector3 GetValidRandomPosition()
        {
            Vector3 candidate = GetRandomPosition();
            while (!IsPositionValid(candidate))
            {
                candidate = GetRandomPosition();
            }
            return candidate;
        }
    }
}
