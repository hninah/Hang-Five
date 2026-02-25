using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentStage = 1;
    public int highScore = 0;
    public bool stageCleared = false;
    [SerializeField] private List<GameObject> allObstaclePrefabs;
    [SerializeField] private List<int> scoreRequired;

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

    public bool GameOver(int finalScore)
    {
        if (finalScore > highScore)
        {
            highScore = finalScore;
        }
        if (currentStage > scoreRequired.Count)
        {
            print("all cutscenes are finished so infinite mode");
            stageCleared = false;
            return false;
        }
        else if (finalScore >= scoreRequired[currentStage - 1])
        {
            // check if player got enough score to pass the stage
            print("stage passed");
            currentStage++;
            stageCleared = true;
            return true;
            
        } else
        {
            print("stage failed");
            return false;
        }
    }
}
