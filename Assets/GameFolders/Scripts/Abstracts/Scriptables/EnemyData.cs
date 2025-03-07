using HHGArchero.Enemy;
using UnityEngine;

namespace HHGArchero.Scriptables
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Data/Enemy Data")]
    public class EnemyData : ScriptableObject
    { 
        [Header("Enemy Pool Settings")]
        [Tooltip("Prefab to pool. Must have an EnemyController component.")]
        [SerializeField] private EnemyController pooledObject;
        [Tooltip("Initial number of objects in the pool.")]
        [SerializeField] private int enemyPoolSize = 5;
        [Tooltip("Minimum X and Z coordinates of the spawn area.")]
        [SerializeField] private Vector2 spawnAreaMin = new Vector2(-4f, -8f);
        [Tooltip("Maximum X and Z coordinates of the spawn area.")]
        [SerializeField] private Vector2 spawnAreaMax = new Vector2(4f, 8f);
        [Tooltip("Minimum allowed distance between spawned objects.")]
        [SerializeField] private float minimumDistanceBetweenEnemies = 2f;
        [Tooltip("Minimum allowed distance from the player for spawned enemies.")]
        [SerializeField] private float minimumDistanceFromPlayer = 5f;
        
        public EnemyController PooledObject => pooledObject;
        public int EnemyPoolSize => enemyPoolSize;
        public Vector2 SpawnAreaMin => spawnAreaMin;
        public Vector2 SpawnAreaMax => spawnAreaMax;
        public float MinimumDistanceBetweenEnemies => minimumDistanceBetweenEnemies;
        public float MinimumDistanceFromPlayer => minimumDistanceFromPlayer;
    }
}