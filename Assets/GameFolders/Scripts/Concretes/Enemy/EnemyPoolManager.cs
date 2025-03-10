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

        private readonly List<Transform> _activeEnemies = new List<Transform>();
        public List<Transform> ActiveEnemies => _activeEnemies;

        protected override void Awake()
        {
            base.Awake();
            InitializeData();
            InitializePool();
        }

        private void Start()
        {
            InitializePlayerTransform();
            SpawnInitialEnemies();
        }

        private void InitializeData()
        {
            _enemyData = DataManager.Instance.EnemyData;
            _enemyPoolSize = _enemyData.EnemyPoolSize;
            _spawnAreaMin = _enemyData.SpawnAreaMin;
            _spawnAreaMax = _enemyData.SpawnAreaMax;
            _minimumDistanceBetweenEnemies = _enemyData.MinimumDistanceBetweenEnemies;
            _minimumDistanceFromPlayer = _enemyData.MinimumDistanceFromPlayer;
            _pooledObject = _enemyData.PooledObject;
        }

        private void InitializePool()
        {
            _objectPool = new ObjectPool<EnemyController>(_pooledObject, _enemyPoolSize, transform);
        }
        
        private void InitializePlayerTransform() => _playerTransform = PlayerController.Instance.transform;

        private void SpawnInitialEnemies()
        {
            for (int i = 0; i < _enemyPoolSize; i++)
            {
                SpawnAtRandomPosition();
            }
        }
        
        private void SpawnAtRandomPosition()
        {
            EnemyController pooledObj = _objectPool.Get();
            pooledObj.transform.position = GetValidRandomPosition();
            Vector3 targetPosition = new Vector3(_playerTransform.position.x, pooledObj.transform.position.y, _playerTransform.position.z);
            pooledObj.transform.LookAt(targetPosition);
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
        
        public void ReturnAndRespawn(EnemyController obj)
        {
            // Remove the enemy from the active list before returning it to the pool.
            if (_activeEnemies.Contains(obj.transform))
                _activeEnemies.Remove(obj.transform);

            _objectPool.ReturnToPool(obj);
            SpawnAtRandomPosition();
        }
    }
}
