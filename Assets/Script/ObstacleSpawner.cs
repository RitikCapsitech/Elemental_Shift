using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Prefabs")]
    public GameObject[] obstaclePrefabs;

    [Header("Spawn Settings")]
    public Transform player;
    public float groundY = -3.5f;

    public float minDistance = 6f;
    public float maxDistance = 10f;

    private float nextSpawnX;
    [Header("Cleanup Settings")]
    public float destroyDistance = 30f;
    private List<GameObject> spawnedObstacles = new List<GameObject>();
    private Vector3 initialNextSpawnX;

    void Start()
    {
        initialNextSpawnX = new Vector3(player.position.x + 8f, 0, 0);
        nextSpawnX = initialNextSpawnX.x;
    }

    void Update()
    {
        // Only spawn if game is running
        if (!GameManager.Instance.IsGameStarted) return;

        if (player.position.x + 20f > nextSpawnX)
        {
            SpawnObstacle();
        }
        CleanupObstacles();
    }

    void SpawnObstacle()
    {
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        Vector3 spawnPos = new Vector3(nextSpawnX, groundY, 0f);

        GameObject obstacle = Instantiate(prefab, spawnPos, Quaternion.identity);
        spawnedObstacles.Add(obstacle);

        float randomGap = Random.Range(minDistance, maxDistance);
        nextSpawnX += randomGap;
    }

    void CleanupObstacles()
    {
        for (int i = spawnedObstacles.Count - 1; i >= 0; i--)
        {
            if (spawnedObstacles[i] == null) continue;

            // If obstacle is behind player by destroyDistance
            if (spawnedObstacles[i].transform.position.x < player.position.x - destroyDistance)
            {
                Destroy(spawnedObstacles[i]);
                spawnedObstacles.RemoveAt(i);
            }
        }
    }

    public void ResetSpawner()
    {
        // Reset spawn position to initial value
        nextSpawnX = initialNextSpawnX.x;
    }

    public void ClearAllObstacles()
    {
        // Destroy all spawned obstacles
        for (int i = spawnedObstacles.Count - 1; i >= 0; i--)
        {
            if (spawnedObstacles[i] != null)
                Destroy(spawnedObstacles[i]);
        }
        spawnedObstacles.Clear();
    }
}
