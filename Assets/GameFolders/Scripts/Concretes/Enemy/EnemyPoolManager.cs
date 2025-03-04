using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{

    [Header("Prefab & Pool Settings")]
    [Tooltip("Prefab to pool. Must have an EnemyController component.")]
    [SerializeField] private GameObject pooledPrefab;

    [Tooltip("Initial number of objects in the pool.")]
    [SerializeField] private int initialPoolSize = 5;

    [Header("Spawn Area Settings")]
    [Tooltip("Minimum X and Z coordinates of the spawn area.")]
    [SerializeField] private Vector2 spawnAreaMin = new Vector2(-10f, -10f);

    [Tooltip("Maximum X and Z coordinates of the spawn area.")]
    [SerializeField] private Vector2 spawnAreaMax = new Vector2(10f, 10f);

    [Header("Spawn Delay")]
    [Tooltip("Delay before an object is respawned after being returned to the pool.")]
    [SerializeField] private float spawnDelay = 1f;

    [Header("Spawn Constraints")]
    [Tooltip("Minimum allowed distance between spawned objects.")]
    [SerializeField] private float minimumDistance = 2f;

    [Header("Player Spawn Constraint")]
    [Tooltip("Reference to the player transform.")]
    [SerializeField] private Transform playerTransform;

    [Tooltip("Minimum allowed distance from the player for spawned enemies.")]
    [SerializeField] private float minimumDistanceFromPlayer = 5f;

    private ObjectPool<EnemyController> _objectPool;

    private void Awake()
    {
        EnemyController prefabComponent = pooledPrefab.GetComponent<EnemyController>();
        if (prefabComponent == null)
        {
            Debug.LogError("The pooled prefab must have an EnemyController component.", this);
            return;
        }

        _objectPool = new ObjectPool<EnemyController>(prefabComponent, initialPoolSize, transform);
    }

    private void Start()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            SpawnAtRandomPosition();
        }
    }

    public void ReturnAndRespawn(EnemyController obj)
    {
        _objectPool.ReturnToPool(obj);
        StartCoroutine(RespawnAfterDelay());
    }

    private void SpawnAtRandomPosition()
    {
        EnemyController pooledObj = _objectPool.Get();
        pooledObj.transform.position = GetValidRandomPosition();
        pooledObj.SetSpawnManager(this);
    }

    private Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomZ = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        return new Vector3(randomX, 1f, randomZ);
    }

    private bool IsPositionValid(Vector3 candidate)
    {
        if (transform.Cast<Transform>().Where(child => child.gameObject.activeInHierarchy).Any(child => Vector3.Distance(child.position, candidate) < minimumDistance))
        {
            return false;
        }

        return !playerTransform || !(Vector3.Distance(playerTransform.position, candidate) < minimumDistanceFromPlayer);
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

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
        SpawnAtRandomPosition();
    }

}
