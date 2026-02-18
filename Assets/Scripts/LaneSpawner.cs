using System.Collections.Generic;
using UnityEngine;

public class LaneSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> obstaclePrefabs;
    [SerializeField] float spawnX = 12f;
    [SerializeField] float minSpawnY = 0.0f;
    [SerializeField] float maxSpawnY = 0.0f;
    [SerializeField] float minDelay = 0.6f;
    [SerializeField] float maxDelay = 1.3f;

    float spawnTimer;

    void Start()
    {
        spawnTimer = Random.Range(minDelay, maxDelay);

        foreach (GameObject prefab in obstaclePrefabs)
        {
            Obstacle obstacle = prefab.GetComponent<Obstacle>();

            if (obstacle == null)
            {
                Debug.LogError("Obstacle: " + obstacle + " does not have an Obstacle script attached. Cannot set y boundaries.");
                continue;
            }

            obstacle.setYBounds(minSpawnY, maxSpawnY);
        }
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            GameObject obstacleType = obstaclePrefabs[getObstacleIndex()];

            float spawnY = getObstacleSpawnY();
            Vector3 pos = new Vector3(spawnX, spawnY, 0f);

            Instantiate(obstacleType, pos, Quaternion.identity);
            spawnTimer = Random.Range(minDelay, maxDelay);
        }
    }

    int getObstacleIndex()
    {
        return Random.Range(0, obstaclePrefabs.Count);
    }

    float getObstacleSpawnY()
    {
        return Random.Range(minSpawnY, maxSpawnY);
    }
}
