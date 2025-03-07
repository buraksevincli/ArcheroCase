using HHGArchero.Projectile;
using UnityEngine;

namespace HHGArchero.Scriptables
{
    [CreateAssetMenu(fileName = "ProjectileData", menuName = "Data/Projectile Data")]
    public class ProjectileData : ScriptableObject
    {
        [Header("Projectile Settings")]
        [Tooltip("Prefab to be pooled; must have a ProjectileController component attached.")]
        [SerializeField] private ProjectileController projectilePrefab;
        [Tooltip("Number of projectile instances maintained in the pool.")]
        [SerializeField] private int poolSize = 10;
        [Tooltip("Launch angle (in degrees) used for calculating the projectile's trajectory.")]
        [SerializeField] private int projectileAngle = 30;
        [Tooltip("Lifetime of the projectile in seconds before it automatically returns to the pool.")]
        [SerializeField] private int projectileLifeTime = 3;
        [Tooltip("Time interval (in seconds) between consecutive projectile fires (fire rate).")]
        [SerializeField] private float fireRate = 0.75f;
        [Tooltip("Delay (in seconds) between consecutive projectiles when firing multiple arrows sequentially.")]
        [SerializeField] private float projectileDelay = 0.2f;
        [Tooltip("Base damage inflicted by the projectile upon impact.")]
        [SerializeField] private int projectileDamage = 10;
        [Tooltip("Damage per tick of burn effect inflicted by the projectile.")]
        [SerializeField] private int projectileBurnDamage = 10;
        
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