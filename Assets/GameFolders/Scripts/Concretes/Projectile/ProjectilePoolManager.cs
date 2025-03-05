using HHGArchero.Managers;
using HHGArchero.ObjectPool;
using HHGArchero.Scriptables;
using HHGArchero.Utilities;

namespace HHGArchero.Projectile
{
    public class ProjectilePoolManager : MonoSingleton<ProjectilePoolManager>
    {
        private GameData _gameData;
        private ProjectileController _projectilePrefab;
        private int _poolSize;
        private ObjectPool<ProjectileController> _projectilePool;

        protected override void Awake()
        {
            base.Awake();
            // Data Initialization
            _gameData = DataManager.Instance.GameData;
            _projectilePrefab = _gameData.ProjectilePrefab;
            _poolSize = _gameData.PoolSize;
            // Pool Initialization
            _projectilePool = new ObjectPool<ProjectileController>(_projectilePrefab, _poolSize, transform);
        }

        public ProjectileController GetProjectile()
        {
            return _projectilePool.Get();
        }

        public void ReturnProjectile(ProjectileController projectile)
        {
            _projectilePool.ReturnToPool(projectile);
        }
    }
}