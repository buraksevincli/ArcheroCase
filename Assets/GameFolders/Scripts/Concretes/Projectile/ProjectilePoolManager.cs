using HHGArchero.Managers;
using HHGArchero.ObjectPool;
using HHGArchero.Scriptables;
using HHGArchero.Utilities;

namespace HHGArchero.Projectile
{
    public class ProjectilePoolManager : MonoSingleton<ProjectilePoolManager>
    {
        private ProjectileData _projectileData;
        private ProjectileController _projectilePrefab;
        private int _poolSize;
        private ObjectPool<ProjectileController> _projectilePool;

        protected override void Awake()
        {
            base.Awake();
            InitializeData();
            InitializePool();
        }
        
        private void InitializeData()
        {
            _projectileData = DataManager.Instance.ProjectileData;
            _projectilePrefab = _projectileData.ProjectilePrefab;
            _poolSize = _projectileData.PoolSize;
        }

        private void InitializePool()
        {
            _projectilePool = new ObjectPool<ProjectileController>(_projectilePrefab, _poolSize, transform);
        }
        public ProjectileController GetProjectile() => _projectilePool.Get();
        public void ReturnProjectile(ProjectileController projectile) => _projectilePool.ReturnToPool(projectile);
        
    }
}