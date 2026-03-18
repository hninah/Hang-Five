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
    [SerializeField] private List<GameObject> activeObstaclePrefabs;

    [Header("Starting Pattern")]
    [SerializeField] private Pattern pattern; //obstacle spawning pattern

    public float MinSpawnY { get{ return minSpawnY; } }
    public float MaxSpawnY { get{ return maxSpawnY; } }

    void Start()
    {
        spawnTimer = Random.Range(minDelay, maxDelay);
        
        minDelay = progressiveMinDelay[progressiveIndex];
        maxDelay = progressiveMaxDelay[progressiveIndex];

        // get the obstacles for whichever stage we are in
        activeObstaclePrefabs = GameManager.Instance.GetActiveObstacles();

        // probabilities
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

        if (spawnTimer <= 0f && activeObstaclePrefabs.Count > 0)
        {
            GameObject obstacleType = activeObstaclePrefabs[getObstacleIndex()];

            float spawnY = pattern.patternSpawnY();

            if (pattern.shouldSpawn()){
                Vector3 pos = new Vector3(spawnX, spawnY, 0f);

                Instantiate(obstacleType, pos, Quaternion.identity);
            }

            //get custom time from the pattern when the timer's paused
            if( pattern.isTimerPaused() ){
                spawnTimer = pattern.getTimer();
            }
            //default timer when pattern isn't controlling the timer
            else{
                spawnTimer = Random.Range(minDelay, maxDelay);
            }
        }
    }


    int getObstacleIndex()
    {

        if (activeObstaclePrefabs.Count > 0){

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


    public void setSpawnPattern(Pattern newPattern){
        pattern = newPattern;
    }

}
