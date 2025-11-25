using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TargetEnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private float baseSpawnRate = 1f;
    private float currentSpawnRate;
    private float spawnTimer = 0f;

    [SerializeField] private int initialPoolSize = 5;
    [SerializeField] private int maxPoolSize = 10;

    private ObjectPool<Enemy> enemyPool;
    private List<Enemy> activeEnemies = new List<Enemy>();

    private void Awake()
    {
        currentSpawnRate = baseSpawnRate;

        enemyPool = new ObjectPool<Enemy>(
            () => Instantiate(enemyPrefab),
            OnGetEnemy, OnReleaseEnemy, OnDestroyEnemy,
            false, initialPoolSize, maxPoolSize
        );

        for (int i = 0; i < initialPoolSize; i++)
        {
            Enemy e = enemyPool.Get();
            enemyPool.Release(e);
        }
    }

    private void Update()
    {
        activeEnemies.RemoveAll(e => e == null || !e.gameObject.activeSelf);

        spawnTimer += Time.deltaTime;

        if (activeEnemies.Count == 0 && spawnTimer >= (1f / currentSpawnRate))
        {
            SpawnTarget();
            spawnTimer = 0f;
        }
    }

    public void SpawnTarget()
    {
        Vector3 spawnPos = GetRandomSpawnPointPosition();

        Enemy enemy = enemyPool.Get();
        enemy.transform.position = spawnPos;
        enemy.transform.rotation = Quaternion.identity; // or some default rotation
        enemy.Init();

        enemy.OnDeath += HandleEnemyDeath;
        activeEnemies.Add(enemy);
    }

    public void UpdateSpawnRate(float timeRemaining)
    {
        currentSpawnRate = baseSpawnRate + timeRemaining;
    }

    public Vector3 GetRandomSpawnPointPosition()
    {
        if (spawnPoints == null || spawnPoints.Count == 0)
            return Vector3.zero;
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Count)];
        return point.position;
    }

    public void ClearAllTargets()
    {
        foreach (Enemy e in activeEnemies)
        {
            if (e != null)
            {
                e.OnDeath -= HandleEnemyDeath;
                enemyPool.Release(e);
            }
        }
        activeEnemies.Clear();
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        enemy.OnDeath -= HandleEnemyDeath;
        enemyPool.Release(enemy);
    }

    private void OnGetEnemy(Enemy enemy) => enemy.gameObject.SetActive(true);
    private void OnReleaseEnemy(Enemy enemy) => enemy.gameObject.SetActive(false);
    private void OnDestroyEnemy(Enemy enemy) => Destroy(enemy.gameObject);
}
