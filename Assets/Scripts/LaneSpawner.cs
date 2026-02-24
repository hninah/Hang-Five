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
    // Temp difficulty scaling
    [SerializeField] List<float> progressiveMinDelay;
    [SerializeField] List<float> progressiveMaxDelay;
    [SerializeField] List<float> progressiveTimeChanges;
    int progressiveIndex = 0;

    float spawnTimer;

    [SerializeField] List<float> obstacleProbs;

    void Start()
    {
        spawnTimer = Random.Range(minDelay, maxDelay);

        minDelay = progressiveMinDelay[progressiveIndex];
        maxDelay = progressiveMaxDelay[progressiveIndex];

        foreach (GameObject prefab in obstaclePrefabs)
        {
            Obstacle obstacle = prefab.GetComponent<Obstacle>();

            if (obstacle == null)
            {
                Debug.LogError("Obstacle: " + obstacle + " does not have an Obstacle script attached. Cannot set y boundaries.");
                continue;
            }

            if (obstacle.name == "Seagull")
            {
                obstacle.setYBounds(-4.5f, -2.0f);
            }
            else
            {
                obstacle.setYBounds(minSpawnY, maxSpawnY);
            }

            if (obstacleProbs.Count != obstaclePrefabs.Count)
            {
                Debug.LogError("Obstacle Probabilities does not match number of obstacles. Manually assigning equal probabilities");
                float prob = 1 / obstaclePrefabs.Count;
                obstacleProbs = new List<float>(obstaclePrefabs.Count);

                for (int i = 0; i < obstacleProbs.Count; ++i)
                {
                    obstacleProbs[i] = prob;
                }
            }
        }
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        // Temp
        progressiveTimeChanges[progressiveIndex] -= Time.deltaTime;
        if (progressiveIndex + 1 < progressiveTimeChanges.Count && progressiveTimeChanges[progressiveIndex] <= 0.0f)
        {
            progressiveIndex += 1;
            minDelay = progressiveMinDelay[progressiveIndex];
            maxDelay = progressiveMaxDelay[progressiveIndex];
        }

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
        float prob = Random.Range(0.0f, 1.0f);
        float currentProb = 0.0f;
        for (int i = 0; i < obstaclePrefabs.Count; ++i)
        {
            if (prob >= currentProb && prob < currentProb + obstacleProbs[i])
            {
                return i;
            }

            currentProb += obstacleProbs[i];
        }

        return 0;
    }

    float getObstacleSpawnY()
    {
        return Random.Range(minSpawnY, maxSpawnY);
    }
}
