using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TargetEnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private Enemy[] enemyPrefab;

    [SerializeField] private float baseSpawnRate = 1f;
    [SerializeField] private float spawnRateIncreasePerSecond = 0.1f;
    private float currentSpawnRate;
    private float spawnTimer = 0f;
    private float saveTime;

    [SerializeField] private int initialPoolSize = 5;
    [SerializeField] private int maxPoolSize = 10;

    private ObjectPool<Enemy> enemyPool;
    private List<Enemy> activeEnemies = new List<Enemy>();

    private void Awake()
    {
        currentSpawnRate = baseSpawnRate;

        enemyPool = new ObjectPool<Enemy>(
            () => 
            {
                int idx = Random.Range(0, enemyPrefab.Length);
                return Instantiate(enemyPrefab[idx]);
            },
            OnGetEnemy, 
            OnReleaseEnemy, 
            OnDestroyEnemy,
            false, 
            initialPoolSize, 
            maxPoolSize
        );

        for (int i = 0; i < initialPoolSize; i++)
        {
            Enemy e = enemyPool.Get();
            enemyPool.Release(e);
        }
    }

    public void OnEnable()
    {
        saveTime = Time.time;
    }
    private void Update()
    {
        float t = Time.time - saveTime;
        UpdateSpawnRate(t * spawnRateIncreasePerSecond);

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

        enemy.OnDeath += HandleEnemyDeath;
        activeEnemies.Add(enemy);
    }

    public void UpdateSpawnRate(float timeRemaining)
    {
        currentSpawnRate = baseSpawnRate + timeRemaining;
    }

    public Vector3 GetRandomSpawnPointPosition()
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Count)];
        return point.position;
    }

    public void ClearAllTargets()
    {
        foreach (Enemy e in activeEnemies)
        {
            if (e != null)
            {
                HandleEnemyDeath(e);
            }
        }
        activeEnemies.Clear();
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        activeEnemies.RemoveAll(e => e == null || !e.gameObject.activeSelf);
        enemy.OnDeath -= HandleEnemyDeath;
        enemyPool.Release(enemy);
    }

    private void OnGetEnemy(Enemy enemy) => enemy.gameObject.SetActive(true);
    private void OnReleaseEnemy(Enemy enemy) => enemy.gameObject.SetActive(false);
    private void OnDestroyEnemy(Enemy enemy) => Destroy(enemy.gameObject);
}
