using System.Collections.Generic;
using UnityEngine;

public class LaneSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject tutorialPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnX = 12f;
    [SerializeField] private float minSpawnY = -4.5f;
    [SerializeField] private float maxSpawnY = 2.1f;

    [Header("Gameplay Timing")]
    [SerializeField] private float gameplayMinDelay = 0.7f;
    [SerializeField] private float gameplayMaxDelay = 2f;

    [Header("Tutorial Timing")]
    [SerializeField] private float tutorialMinDelay = 2.5f;
    [SerializeField] private float tutorialMaxDelay = 3.5f;
    [SerializeField] List<float> obstacleProbs;
    [SerializeField] private List<GameObject> allObstaclePrefabs;
    [SerializeField] private List<GameObject> activeObstaclePrefabs; ///

    [Header("Starting Pattern")]
    [SerializeField] private Pattern pattern; //obstacle spawning pattern

    public float MinSpawnY { get{ return minSpawnY; } }
    public float MaxSpawnY { get{ return maxSpawnY; } }

    void Start()
    {
        spawnTimer = Random.Range(minDelay, maxDelay);
        
        minDelay = progressiveMinDelay[progressiveIndex];
        maxDelay = progressiveMaxDelay[progressiveIndex];

    private float spawnTimer;
    private bool tutorialMode = false;
    float GetRandomDelay()
    {
        // different speed and time between spawn for tutorial
        if (tutorialMode)
            return Random.Range(tutorialMinDelay, tutorialMaxDelay);
        // get the obstacles for whichever stage we are in
        activeObstaclePrefabs = GameManager.Instance.GetCheckpointObstacles();

        // probabilities
        if (obstacleProbs.Count != activeObstaclePrefabs.Count)
        {
            obstacleProbs = new List<float>();
            float equalProb = 1f / activeObstaclePrefabs.Count;

        return Random.Range(gameplayMinDelay, gameplayMaxDelay);
    }

    void OnEnable()
    {
        spawnTimer = GetRandomDelay();
    }


    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnObstacle();
            spawnTimer = GetRandomDelay();
        }
    }

    void SpawnObstacle()
    {
        GameObject prefab;
        // look for tutorial obstacle
        if (tutorialMode && tutorialPrefab != null)
        {
            prefab = tutorialPrefab;
        }
        else
        if (spawnTimer <= 0f && activeObstaclePrefabs.Count > 0)
        {
            ///Debug.Log("LS: spawning new obstacles");
            GameObject obstacleType = activeObstaclePrefabs[getObstacleIndex()];
            // wait for gameplay scene to load and add the obstacles for that stage
            List<GameObject> stageObstacles = GameManager.Instance.GetStageObstacles();

            ///float spawnY = getObstacleSpawnY();
            float spawnY = pattern.patternSpawnY();

            if (pattern.shouldSpawn()){
                Vector3 pos = new Vector3(spawnX, spawnY, 0f);
            if (stageObstacles == null || stageObstacles.Count == 0)
                return;

            prefab = stageObstacles[Random.Range(0, stageObstacles.Count)];
        }
                Instantiate(obstacleType, pos, Quaternion.identity);
            }
            ///else Debug.Log("LS: don't spawn this obstacle");

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
        float y = Random.Range(minSpawnY, maxSpawnY);

        GameObject obstacle = Instantiate(prefab, new Vector3(spawnX, y, 0f), Quaternion.identity);

        Obstacle obs = obstacle.GetComponent<Obstacle>();

        if (obs != null)
        {
            // tutorial obstacles move at half the speed
            if (tutorialMode)
            {
                obs.scrollSpeed = 2.5f;
            } else
            {
                // regular obstacle speed
                obs.scrollSpeed = 5f;
            }
        }
    }

    public void EnableTutorialMode()
    {
        tutorialMode = true;
    }


    public void setSpawnPattern(Pattern newPattern){
        pattern = newPattern;
    public void DisableTutorialMode()
    {
        tutorialMode = false;
    }

}

}