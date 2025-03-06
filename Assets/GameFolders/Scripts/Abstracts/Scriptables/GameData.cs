using HHGArchero.Enemy;
using HHGArchero.Projectile;
using UnityEngine;

namespace HHGArchero.Scriptables
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Data/Game Data")]
    public class GameData : ScriptableObject
    {
        [Header("Player Settings")]
        [SerializeField] private int moveSpeed = 5;
        
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
        
        [Header("Projectile Settings")]
        [Tooltip("Prefab to pool. Must have an ProjectileController component.")]
        [SerializeField] private ProjectileController projectilePrefab;
        [SerializeField] private int poolSize = 10;
        [SerializeField] private int projectileAngle = 30;
        [SerializeField] private int projectileLifeTime = 3;
        [SerializeField] private float fireRate = .75f;
        [SerializeField] private float projectileDelay = .2f;
        [SerializeField] private int projectileDamage = 10;
        [SerializeField] private int projectileBurnDamage = 10;
        
        public int MoveSpeed => moveSpeed;
        public EnemyController PooledObject => pooledObject;
        public int EnemyPoolSize => enemyPoolSize;
        public Vector2 SpawnAreaMin => spawnAreaMin;
        public Vector2 SpawnAreaMax => spawnAreaMax;
        public float MinimumDistanceBetweenEnemies => minimumDistanceBetweenEnemies;
        public float MinimumDistanceFromPlayer => minimumDistanceFromPlayer;
        public ProjectileController ProjectilePrefab => projectilePrefab;
        public int PoolSize => poolSize;
        public int ProjectileAngle => projectileAngle;
        public int ProjectileLifeTime => projectileLifeTime;
        public float FireRate => fireRate;
        public float ProjectileDelay => projectileDelay;
        public int ProjectileDamage => projectileDamage;
        public int ProjectileBurnDamage => projectileBurnDamage;
    }
}
