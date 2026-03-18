using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    //public variables
    public LaneSpawner laneSpawner;

    [Header("Possible Spawn Patterns")]
    [SerializeField] private List<Pattern> allSpawnPatterns;

    //min and max delay before we choose a new pattern
    [Header("Delay Before Choosing a New Pattern")]
    [SerializeField] float minPatternDelay = 5f;
    [SerializeField] float maxPatternDelay = 10f;
    
    //private variables
    //track the current checkpoint
    private List<int> patternCheckpoints = new List<int>();
    private int latestPattCheckpoint;
    private int latestPattCheckpointIndex;
    private int nextPattCheckpoint;

    //time to wait before choosing a new pattern
    private float patternTimer;

    //use these to choose a new pattern
    private List<Pattern> activeSpawnPatterns = new List<Pattern>();
    private List<float> patternProbs = new List<float>();
    

    void Start(){

        //set up pattern checkpoints
        getPatternCheckpoints();

        //start the first pattern and timer
        getActivePatterns();
        patternTimer = Random.Range(minPatternDelay, maxPatternDelay);

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

        //if timer reaches 0, choose a new pattern
        if (patternTimer <= 0f && activeSpawnPatterns.Count > 0)
        {
            //reset the LaneSpawner's pattern
            laneSpawner.setSpawnPattern( activeSpawnPatterns[getPatternIndex()] );

            //restart timer
            patternTimer = Random.Range(minPatternDelay, maxPatternDelay);
        }
    }


    void getActivePatterns(){

        activeSpawnPatterns.Clear();

        //all patterns from earlier checkpoints are also active
        foreach (Pattern p in allSpawnPatterns){

            int checkpointP = p.getCheckpointCutscene();
            //to display this pattern need:
            // - we reached the checkpoint where this pattern appears
            // - we passed the cutscene associated with that checkpoint
            if (p != null && (checkpointP <= latestPattCheckpoint) 
                        && (CutsceneManager.Instance.getCurrentCutscene() > checkpointP))
            {
                activeSpawnPatterns.Add(p);
            }
        }
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

        //set the latest and next checkpoints
        int currCutscene = CutsceneManager.Instance.getCurrentCutscene();

        for (int ptIndex = 0; ptIndex < patternCheckpoints.Count; ++ptIndex){

            int checkpt = patternCheckpoints[ptIndex];

            //find the latest checkpoing we passed
            if (currCutscene >= checkpt){
                latestPattCheckpointIndex = ptIndex;
                latestPattCheckpoint = checkpt;

                //find the next checkpoint if possible
                if (ptIndex + 1 < patternCheckpoints.Count){
                    nextPattCheckpoint = patternCheckpoints[ ptIndex + 1];
                }
            } 
        }
    }

}
