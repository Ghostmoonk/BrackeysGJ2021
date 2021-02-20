using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn position")]
    [Tooltip("Transform to spawn around"), SerializeField]
    Transform spawnCenter;
    [Tooltip("Transform to spawn around"), SerializeField]
    float minSpawnRadiusAround;
    [Tooltip("Transform to spawn around"), SerializeField]
    float maxSpawnRadiusAround;

    [Header("Enemy")]
    [Tooltip("Spawn every X seconds"), SerializeField]
    float spawnPerSecond;
    float currentSpawnRateMultiplier;
    [SerializeField]
    GameObject enemyPrefab;

    [SerializeField]
    bool hasSpawnLimitation;
    [SerializeField]
    int maxSpawnedEnemyAmont;
    List<Enemy> enemySpawnedList;

    bool shouldSpawn;

    float spawnTimer;

    private void Start()
    {
        shouldSpawn = true;
        spawnTimer = 0f;
        currentSpawnRateMultiplier = 1f;

        enemySpawnedList = new List<Enemy>();
    }

    private void Update()
    {
        if (shouldSpawn)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer > spawnPerSecond * currentSpawnRateMultiplier)
            {
                Vector2 spawnPosition = GetRandomPositionAroundCenter();
                Enemy spawnedEnemy = Instantiate(enemyPrefab,
                    new Vector3(spawnPosition.x, spawnCenter.position.y, spawnPosition.y) + spawnCenter.position,
                    Quaternion.identity,
                    GameObject.FindGameObjectWithTag("Enemies").transform).GetComponent<Enemy>();
                spawnTimer -= spawnPerSecond * currentSpawnRateMultiplier;
                enemySpawnedList.Add(spawnedEnemy);

                spawnedEnemy.OnDieEvent += RemoveEnemy;
            }
        }

        if (hasSpawnLimitation && enemySpawnedList.Count >= maxSpawnedEnemyAmont)
            shouldSpawn = false;
        else if (shouldSpawn == false && hasSpawnLimitation && enemySpawnedList.Count < maxSpawnedEnemyAmont)
            shouldSpawn = true;
    }

    private Vector2 GetRandomPositionAroundCenter()
    {
        float r = Random.Range(0f, 1f);
        Vector2 randomPos = new Vector3(Mathf.Cos(r * Mathf.PI * 2), Mathf.Sin(r * Mathf.PI * 2)) * Random.Range(minSpawnRadiusAround, maxSpawnRadiusAround);
        return randomPos;
    }

    public void RemoveEnemy(Enemy enemy) => enemySpawnedList.Remove(enemy);

    public void SetCurrentTimeMultiplier(float newSpawnRateMultiplier) => currentSpawnRateMultiplier = newSpawnRateMultiplier;
}
