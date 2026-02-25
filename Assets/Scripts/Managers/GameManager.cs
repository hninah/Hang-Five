using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentStage = 1;
    [SerializeField] private List<GameObject> allObstaclePrefabs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<GameObject> GetStageObstacles()
    {
        // get the obstacles for each stage in the game manager rather than making lane spawner decide that
        List<GameObject> obstacles = new List<GameObject>();

        foreach (GameObject prefab in allObstaclePrefabs)
        {
            Obstacle obs = prefab.GetComponent<Obstacle>();

            if (obs != null && obs.getStage() <= currentStage)
            {
                obstacles.Add(prefab);
            }
        }
        print("Current Stage: " + currentStage);
        return obstacles;
    }
}
