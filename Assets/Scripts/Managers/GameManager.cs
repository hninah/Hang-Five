using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentStage = 0;
    public int highScore = 0;
    public bool stageCleared = false;
    [SerializeField] private List<GameObject> allObstaclePrefabs;
    [SerializeField] private List<int> scoreRequired;
    public bool tutorialCompleted = false;

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


    public List<GameObject> GetCheckpointObstacles()
    {
        // get the obstacles for the current checkpoint phase rather than making lane spawner decide that
        List<GameObject> obstacles = new List<GameObject>();

        foreach (GameObject prefab in allObstaclePrefabs)
        {
            Obstacle obs = prefab.GetComponent<Obstacle>();

            ///Debug.Log("GM: checking obstacle " + obs.Name + " with checkpoint cutscene " + obs.getCheckpointCutscene() + ", current cutscene = " + CutsceneManager.Instance.getCurrentCutscene());
            //to display this obstacle need:
            // - we reached the checkpoint where this obstacle appears
            // - we passed the cutscene associated with that checkpoint
            if (obs != null && (obs.getCheckpointCutscene() <= CutsceneManager.Instance.getCurrentCheckpoint()) )
                            ///&& (CutsceneManager.Instance.getCurrentCutscene() > obs.getCheckpointCutscene()))
            {
                ///Debug.Log("GM: valid obstacle " + obs.Name + " has checkpoint cutscene " + obs.getCheckpointCutscene() + " which is <= current checkpoint " + CutsceneManager.Instance.getCurrentCheckpoint());
                obstacles.Add(prefab);
            }
        }

        ///////// PRINT STATEMENT FOR TESTING /////////
        /*
        string printObs = "";
        foreach (GameObject prefab in obstacles){ 
            Obstacle ob = prefab.GetComponent<Obstacle>();
            printObs = printObs + "  " + ob.Name;
        }
        Debug.Log("GM: current checkpoint: " + CutsceneManager.Instance.getCurrentCheckpoint() + ", using [" + printObs + "]");
        */
        /////////////////////////////


        return obstacles;
    }


    public bool GameOver(int finalScore)
    {
        if (finalScore > highScore)
        {
            highScore = finalScore;
        }

        //move to next stage if:
        //  1) we ran out of score thresholds, or
        //  2) player passed the current score threshold
        if ( (currentStage - 1) >= scoreRequired.Count){
            print("stage passed");
            stageCleared = true;
            return true;
        }
        else if (( (currentStage - 1) < scoreRequired.Count) && (finalScore >= scoreRequired[currentStage - 1]))
        {
            // check if player got enough score to pass the stage
            Debug.Log("finalScore = " + finalScore + ", score required = " + scoreRequired[currentStage - 1]);
            currentStage++;
            stageCleared = true;
            return true;
        }else
        {
            print("stage failed");
            return false;
        }
    }


    public List<int> getObstacleCheckpoints(){

        List<int> checkpoints = new List<int>();

        foreach (GameObject obsPrefab in allObstaclePrefabs){
            Obstacle obs = obsPrefab.GetComponent<Obstacle>();

            checkpoints.Add(obs.getCheckpointCutscene());
        }

        ///checkpoints.Sort((a, b) => a.CompareTo(b));
        checkpoints.Sort();

        ///////// PRINT STATEMENT FOR TESTING /////////
        /*
        string printcp = "";
        foreach (int cp in checkpoints){ 
            printcp = printcp + "  " + cp;
        }
        Debug.Log("GM: we have obstacle checkpoints = [" + printcp + "]");
        */
        /////////////////////////////
        
        return checkpoints;
    }
}
