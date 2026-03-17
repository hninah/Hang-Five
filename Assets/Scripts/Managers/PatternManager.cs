using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    //public variables
    public LaneSpawner laneSpawner;

    [Header("Possible Spawn Patterns")]
    [SerializeField] private List<Pattern> allSpawnPatterns;

    ///[Header("Cutscene # Where New Patterns Appear")]
    ///[SerializeField] private List<int> patternCheckpoints___;
    
    //min and max delay before we choose a new pattern
    [Header("Delay Before Choosing a New Pattern")]
    [SerializeField] float minPatternDelay = 5f;
    [SerializeField] float maxPatternDelay = 10f;
    
    //private variables
    //track the current checkpoint
    private List<int> patternCheckpoints = new List<int>();
    private int latestCheckpoint;
    private int latestCheckpointIndex;
    private int nextCheckpoint;

    //time to wait before choosing a new pattern
    private float patternTimer;

    //use these to choose a new pattern
    private List<Pattern> activeSpawnPatterns = new List<Pattern>();
    private List<float> patternProbs = new List<float>();
    

    void Start(){

        //set up pattern checkpoints
        getPatternCheckpoints();

        latestCheckpoint = patternCheckpoints[0];;
        latestCheckpointIndex = 0;
        nextCheckpoint = patternCheckpoints[1];

        //start the first pattern
        getActivePatterns();
        patternTimer = Random.Range(minPatternDelay, maxPatternDelay);
        Debug.Log("PM: start, set timer to " + patternTimer);

        //probabilities
        if (patternProbs.Count != activeSpawnPatterns.Count)
        {
            patternProbs = new List<float>();
            float equalProb = 1f / activeSpawnPatterns.Count;

            for (int i = 0; i < activeSpawnPatterns.Count; i++)
            {
                patternProbs.Add(equalProb);
            }
        }
    }


    void Update(){
        patternTimer -= Time.deltaTime;

        ///////// PRINT STATEMENT FOR TESTING /////////
        /*
        string printPs = "";
        foreach (Pattern p in activeSpawnPatterns){ 
            printPs = printPs + "  " + p.Name;
        }
        Debug.Log("GM: current checkpoint: " + CutsceneManager.Instance.getCurrentCheckpoint() + ", using [" + printPs + "]");
        */
        /////////////////////////////

        //if timer reaches 0, choose a new pattern
        if (patternTimer <= 0f && activeSpawnPatterns.Count > 0)
        {
            //reset the LaneSpawner's pattern
            Pattern newPattern = activeSpawnPatterns[getPatternIndex()]; ///for testing
            ///laneSpawner.setSpawnPattern( activeSpawnPatterns[getPatternIndex()] );
            laneSpawner.setSpawnPattern( newPattern );

            //restart timer
            patternTimer = Random.Range(minPatternDelay, maxPatternDelay);

            ///Debug.Log("PM: new pattern: " + newPattern.Name + ", reset timer to " + patternTimer);
            Debug.Log("PM: new pattern: " + newPattern.Name + ", reset timer to " + patternTimer);
        }
    }


    void getActivePatterns(){

        Debug.Log("PM: called getActivePatterns");

        //check CutsceneManger to find what phase we're in
        int currentPhase = CutsceneManager.Instance.getCurrentCutscene();
        Debug.Log("PM: current cutscene = " + currentPhase + ", nextCheckpoint = " + nextCheckpoint);

        //update the current pattern checkpoint if needed
        if (currentPhase >= nextCheckpoint && (latestCheckpointIndex < patternCheckpoints.Count)){
            //track which checkpoint we most recently passed
            ++latestCheckpointIndex;
            latestCheckpoint = patternCheckpoints[ latestCheckpointIndex ];
            Debug.Log("PM: updating current checkpoint: now latest checkpoint = " + latestCheckpoint);
            
            //if possible, look forward to the new nextCheckpoint
            if ( (latestCheckpointIndex + 1) < patternCheckpoints.Count){
                nextCheckpoint = patternCheckpoints[latestCheckpointIndex + 1];
                Debug.Log("PM: updating next checkpoint: now next checkpoint = " + nextCheckpoint);
            }
            else Debug.Log("PM: can't look forward to next checkpoint");

        }

        activeSpawnPatterns.Clear();
        //all patterns from earlier checkpoints are also active
        Debug.Log("PM: latestCheckpointIndex = " + latestCheckpointIndex);
        
        foreach (Pattern p in allSpawnPatterns){

            Debug.Log("PM: checking pattern " + p.Name + " with checkpoint cutscene " + p.getCheckpointCutscene() + ", current cutscene = " + CutsceneManager.Instance.getCurrentCutscene());
            //to display this pattern need:
            // - we reached the checkpoint where this pattern appears
            // - we passed the cutscene associated with that checkpoint
            ///if (p != null && (p.getCheckpointCutscene() <= CutsceneManager.Instance.getCurrentCheckpoint()) )
                ///            && (CutsceneManager.Instance.getCurrentCutscene() > p.getCheckpointCutscene()))
            if (p != null && (p.getCheckpointCutscene() <= latestCheckpoint) 
                && (CutsceneManager.Instance.getCurrentCutscene() > p.getCheckpointCutscene()))
            {
                ///Debug.Log("PM: valid pattern " + p.Name + " has checkpoint cutscene " + p.getCheckpointCutscene() + " which is <= current checkpoint " + CutsceneManager.Instance.getCurrentCheckpoint());
                Debug.Log("PM: valid pattern " + p.Name + " has checkpoint cutscene " + p.getCheckpointCutscene() + " which is <= current checkpoint " + latestCheckpoint);
                activeSpawnPatterns.Add(p);
            }
        }

        ///////// PRINT STATEMENT FOR TESTING /////////
        string printPs = "";
        int numPs = 0;
        foreach (Pattern p in activeSpawnPatterns){ 
            ///printPs = printPs + "  " + p.Name;
            printPs = printPs + "  " + p.Name;
            ++numPs;
        }
        Debug.Log("PM: current checkpoint: " + CutsceneManager.Instance.getCurrentCheckpoint() + ", using [" + printPs + "], " + numPs +" patterns");
        /////////////////////////////
    }



    int getPatternIndex()
    {

        if (activeSpawnPatterns.Count > 0){

            float prob = Random.Range(0.0f, 1.0f);
            float currentProb = 0.0f;
            for (int i = 0; i < activeSpawnPatterns.Count; ++i)
            {
                if (prob >= currentProb && prob < currentProb + patternProbs[i])
                {
                    return i;
                }

                currentProb += patternProbs[i];
            }
        }
        return 0;
    }

    void getPatternCheckpoints(){

        foreach (Pattern p in allSpawnPatterns){
            patternCheckpoints.Add(p.getCheckpointCutscene());
        }
        patternCheckpoints.Sort();

        ///////// PRINT STATEMENT FOR TESTING /////////
        
        string printcp = "";
        foreach (int cp in patternCheckpoints){ 
            printcp = printcp + "  " + cp;
        }
        Debug.Log("PM: we have pattern checkpoints = [" + printcp + "]");
        
        /////////////////////////////
    }
}
