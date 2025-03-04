using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PoolManager _spawnManager;
    public void SetSpawnManager(PoolManager spawnManager)
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