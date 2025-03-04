using UnityEngine;

namespace HHGArchero.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        private EnemyPoolManager _spawnManager;
        public void SetSpawnManager(EnemyPoolManager spawnManager)
        {
            this._spawnManager = spawnManager;
        }

        private void OnMouseDown()
        {
            if (_spawnManager != null)
            {
                _spawnManager.ReturnAndRespawn(this);
            }
        }
    }
}