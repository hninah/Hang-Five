using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    //public/visible variables
    public static GameManager Instance;

    public int currentStage = 1;
    public int highScore = 0;
    public int targetScore = 0;
    public bool stageCleared = false;
    public bool beatBoss = false;
    [SerializeField] private List<GameObject> allObstaclePrefabs;
    [SerializeField] private List<PatternScriptable> allPatterns;
    [SerializeField] public List<int> scoreRequired;
    public bool tutorialCompleted = false;

    //private variables
    private List<int> obstacleCheckpoints = new List<int>();

    //track which obstacle checkpoint we expect next
    // (this is an index in CutsceneManager's cutsceneList)
    private int nextObsCheckpoint;

    //cutscene number of the last checkpoint we passed
    private int latestObsCheckpoint;
    //index in obstacleCheckpoints of the last checkpoint we passed
    private int latestObsCheckpointIndex;

    //BackgroundSelector.cs sets this, ScoreManager.cs and TextParticle.cs can 
    //  check here to know when to have white text
    private string background;
    public string getBackground() { return background; }
    public void setBackground( string newBackground ) { background = newBackground; }

    // checks if player is in the boss level and if they reach the target score 
    public bool inBossLevel = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            //set up obstacle checkpoints from provided obstacles
            getObstacleCheckpoints();

            //start the latest and next checkpoints
            if (obstacleCheckpoints.Count > 0){
                latestObsCheckpointIndex = 0;
                latestObsCheckpoint = obstacleCheckpoints[0];

                if(obstacleCheckpoints.Count > 1){
                    nextObsCheckpoint = obstacleCheckpoints[1];
                }
                else Debug.Log("There's only one obstacle checkpoint");
            }
            else Debug.Log("Remember to add obstacle checkpoints!");
            
            // initialize the first target score
            targetScore = scoreRequired[0];
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public List<GameObject> GetActiveObstacles()
    {   
        int currentCutscene = CutsceneManager.Instance.getCurrentCutscene();

        //update latest checkpoint and checkpoint index if 
        // - we reached the next checkpoint
        // - we didn't already pass the last checkpoint
        if ( (currentCutscene == nextObsCheckpoint) && (latestObsCheckpointIndex < obstacleCheckpoints.Count -1)){

            //reached the next checkpoint: update current checkpoint index and checkpoint
            ++latestObsCheckpointIndex;
            latestObsCheckpoint = obstacleCheckpoints[latestObsCheckpointIndex];

            //if possible, look forward to the new nextCheckpoint
            if ( (latestObsCheckpointIndex + 1) < obstacleCheckpoints.Count){
                nextObsCheckpoint = obstacleCheckpoints[latestObsCheckpointIndex + 1];
            }
        }

        List<GameObject> obstacles = new List<GameObject>();

        // get the obstacles for the current checkpoint phase rather than making lane spawner decide that
        foreach (GameObject prefab in allObstaclePrefabs)
        {
            Obstacle obs = prefab.GetComponent<Obstacle>();

            //spawn this obstacle if:
            // - we reached the checkpoint where this obstacle appears
            // - we passed the cutscene associated with that checkpoint
            if (obs != null && (obs.getCheckpointCutscene() <= latestObsCheckpoint)
                            && (currentCutscene > obs.getCheckpointCutscene()))
            {   
                //special case: don't show seagull between cutscenes 0 and 1
                if(currentCutscene == 1 && obs.Name == "Seagull"){
                    continue;
                }
                //otherwise add to active obstacle list
                obstacles.Add(prefab);
                
            }
        }
        return obstacles;
    }

    public List<PatternScriptable> GetActivePatterns()
    {
        List<PatternScriptable> output = new List<PatternScriptable>();

        foreach (PatternScriptable pattern in allPatterns)
        {
            for (int i = 0; i < pattern.checkPointCutscenes.Length; ++i)
            {
                if (pattern.checkPointCutscenes[i] != currentStage)
                {
                    continue;
                }

                output.Add(pattern);
                break;
            }
        }

        return output;
    }


    public bool GameOver(int finalScore)
    {
        Debug.Log($"GameOver: stage={currentStage}, inBoss={inBossLevel}, score={finalScore}");
        if (inBossLevel)
        {
            // beat the boss and im no longer in the boss level
            if (finalScore >= scoreRequired[currentStage - 1])
            {
                currentStage++;
                stageCleared = true;
                beatBoss = true;
                inBossLevel = false;
                return true;
            }
            // did not beat the boss
            else
            {
                stageCleared = false;
                return false;
            }

        }
        else
        {
            // no longer in the boss level but in the infinite level
            if (currentStage >= scoreRequired.Count)
            {
                stageCleared = false;

                if (finalScore > highScore)
                {
                    highScore = finalScore;
                }

                return false;
            }
        }
        //move to next stage if:
        //  1) we ran out of score thresholds, or
        //  2) player passed the current score threshold
        // we have not ran out of score thresholds yet
        if (finalScore >= scoreRequired[currentStage - 1])
        {
            currentStage++;
            stageCleared = true;
            // boss occurs at the second last score threshold because boss needs a score threshold too
            if (currentStage == scoreRequired.Count - 1)
            {
                inBossLevel = true;
                targetScore = scoreRequired[currentStage];
                return true;
            }

            // regular next level (not the boss)
            if (currentStage <= scoreRequired.Count)
            {
                targetScore = scoreRequired[currentStage - 1];
            }

            return true;
        }

        stageCleared = false;
        return false;
    }

    //quick check for Player.cs whether to display the "Next" button
    public bool passedCurrentStage(int finalScore){
        return ( (currentStage - 1) >= scoreRequired.Count) || 
            (( (currentStage - 1) < scoreRequired.Count) && (finalScore >= scoreRequired[currentStage - 1]));
    }

    //ScoreManager.cs can use this to set the current threshold text
    public int getCurrentThreshold(){
        return scoreRequired[currentStage - 1];
    }


    private void getObstacleCheckpoints(){

        //get the list of obstacle checkpoints from the provided obstacles
        foreach (GameObject obsPrefab in allObstaclePrefabs){
            Obstacle obs = obsPrefab.GetComponent<Obstacle>();

            int obsPt = obs.getCheckpointCutscene();
            
            if ( !obstacleCheckpoints.Contains(obsPt) ){
                obstacleCheckpoints.Add(obsPt);
            }
        }

        obstacleCheckpoints.Sort();
    }


    //use when we press the Quit button on the wipeout screen
    public void resetGameState(){
        //reset score, boss and tutorial variables
        currentStage = 1;
        highScore = 0;
        stageCleared = false;
        inBossLevel = false;
        beatBoss = false;
        tutorialCompleted = false;

        // reset score thresholds
        if (scoreRequired.Count > 0)
        {
            targetScore = scoreRequired[0];
        }
        setBackground("Day");
        //reset active-obstacle variables
        //start the latest and next checkpoints
        if (obstacleCheckpoints.Count > 0){
            latestObsCheckpoint = obstacleCheckpoints[0];
            latestObsCheckpointIndex = 0;

            if(obstacleCheckpoints.Count > 1){
                nextObsCheckpoint = obstacleCheckpoints[1];
            }
        }
    }
}
