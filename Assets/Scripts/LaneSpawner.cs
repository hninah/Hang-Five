using System.Collections.Generic;
using UnityEngine;

public class LaneSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> obstaclePrefabs;
    ///[SerializeField] float spawnX = 12f;
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
    [SerializeField] private List<GameObject> activeObstaclePrefabs;

    [Header("Starting Pattern")]
    [SerializeField] private Pattern pattern;

    public float MinSpawnY { get { return minSpawnY; } }
    public float MaxSpawnY { get { return maxSpawnY; } }

    void Start()
    {
        if (progressiveMinDelay.Count > 0 && progressiveMaxDelay.Count > 0)
        {
            minDelay = progressiveMinDelay[Mathf.Clamp(progressiveIndex, 0, progressiveMinDelay.Count - 1)];
            maxDelay = progressiveMaxDelay[Mathf.Clamp(progressiveIndex, 0, progressiveMaxDelay.Count - 1)];
        }

        spawnTimer = Random.Range(minDelay, maxDelay);

        // get the obstacles for whichever stage we are in
        activeObstaclePrefabs = GameManager.Instance.GetActiveObstacles();

        if (activeObstaclePrefabs == null)
            activeObstaclePrefabs = new List<GameObject>();

        if (activeObstaclePrefabs.Count > 0)
        {
            if (obstacleProbs.Count != activeObstaclePrefabs.Count)
            {
                obstacleProbs = new List<float>();
                float equalProb = 1f / activeObstaclePrefabs.Count;

                for (int i = 0; i < activeObstaclePrefabs.Count; i++)
                {
                    obstacleProbs.Add(equalProb);
                }
            }
        }
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (progressiveTimeChanges.Count > 0 && progressiveIndex < progressiveTimeChanges.Count)
        {
            progressiveTimeChanges[progressiveIndex] -= Time.deltaTime;

            if (progressiveIndex + 1 < progressiveTimeChanges.Count &&
                progressiveTimeChanges[progressiveIndex] <= 0.0f)
            {
                progressiveIndex += 1;

                if (progressiveMinDelay.Count > progressiveIndex)
                    minDelay = progressiveMinDelay[progressiveIndex];

                if (progressiveMaxDelay.Count > progressiveIndex)
                    maxDelay = progressiveMaxDelay[progressiveIndex];
            }
        }

        if (spawnTimer <= 0f && activeObstaclePrefabs.Count > 0)
        {
            int index = getObstacleIndex();
            if (index < 0 || index >= activeObstaclePrefabs.Count)
                index = 0;

            GameObject obstacleType = activeObstaclePrefabs[index];
            float spawnX = obstacleType.GetComponent<Obstacle>().getSpawnX();

            float spawnY = pattern.patternSpawnY();

            if (pattern.shouldSpawn())
            {
                Vector3 pos = new Vector3(spawnX, spawnY, 0f);
                Instantiate(obstacleType, pos, Quaternion.identity);
            }
            //get custom time from the pattern when the timer's paused
            if (pattern.isTimerPaused())
            {
                spawnTimer = pattern.getTimer();
            }
            //default timer when pattern isn't controlling the timer
            else
            {
                spawnTimer = Random.Range(minDelay, maxDelay);
            }
        }
    }

    int getObstacleIndex()
    {
        if (activeObstaclePrefabs.Count > 0 && obstacleProbs.Count == activeObstaclePrefabs.Count)
        {
            float prob = Random.Range(0.0f, 1.0f);
            float currentProb = 0.0f;

            for (int i = 0; i < activeObstaclePrefabs.Count; ++i)
            {
                if (prob >= currentProb && prob < currentProb + obstacleProbs[i])
                {
                    return i;
                }

                currentProb += obstacleProbs[i];
            }
        }

        return 0;
    }

    public void setSpawnPattern(Pattern newPattern)
    {
        pattern = newPattern;
    }
}