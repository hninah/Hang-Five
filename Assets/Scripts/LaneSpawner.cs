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

    private float spawnTimer;
    private bool tutorialMode = false;
    float GetRandomDelay()
    {
        // different speed and time between spawn for tutorial
        if (tutorialMode)
            return Random.Range(tutorialMinDelay, tutorialMaxDelay);

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
        {
            // wait for gameplay scene to load and add the obstacles for that stage
            List<GameObject> stageObstacles = GameManager.Instance.GetStageObstacles();

            if (stageObstacles == null || stageObstacles.Count == 0)
                return;

            prefab = stageObstacles[Random.Range(0, stageObstacles.Count)];
        }

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

    public void DisableTutorialMode()
    {
        tutorialMode = false;
    }

}