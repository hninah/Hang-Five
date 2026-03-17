using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{

    public static CutsceneManager Instance;

    //check here after wipeout to get the next cutscene
    [Tooltip("All the CutsceneInfos in the order they appear.")]
    public List<CutsceneInfo> cutsceneList;

    //mark which indices are checkpoints for new obstacles
    //eg. if 3 is in this list, new obstacle will show up after cutscene with index 3
    ///[Tooltip("New Obstacles appear after the cutscenes with these numbers (where the number is the cutscene's index in CutsceneList). These numbers correspond to Obstacle's checkpointCutscene variable.")]
    public List<int> obstacleCheckpoints___;
    private List<int> obstacleCheckpoints = new List<int>();

    //track which cutscene we're on
    private int currIndex;
    //track whether all cutscenes were seen yet
    private bool finishedCutscenes;

    //track which obstacle checkpoint we expect next
    // (this is an index in the cutsceneList)
    private int nextCheckpoint;

    //cutscene number (index in cutsceneList) of the last checkpoint we passed
    private int currCheckpoint;
    //index in obstacleCheckpoints of the last checkpoint we passed
    private int currCheckpointIndex; 


    private void Awake(){

        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);

            //start game on the first cutscene
            currIndex = 0;
            //check starting list length to see if we have cutscenes
            if (cutsceneList.Count > 0) finishedCutscenes = false;
            else finishedCutscenes = true;
        }
        else{
            Destroy(gameObject);
        }
    }


    void Start(){
        //Do this in Start so it happens after GameManager is instantiated
        //set up checkpoints for new obstacle types
        if (GameManager.Instance != null){
                obstacleCheckpoints = GameManager.Instance.getObstacleCheckpoints();

            if (obstacleCheckpoints.Count > 0){
                currCheckpoint = obstacleCheckpoints[0];

                if(obstacleCheckpoints.Count > 1){
                    nextCheckpoint = obstacleCheckpoints[1];
                }
            }
        }
        else Debug.Log("GM is null");
    }


    public CutsceneInfo getNextCutscene(){
        //return next cutscene if there is one, update checkpoints
        if(!finishedCutscenes){

            //update current checkpoint and checkpoint index if 
            // - we didn't already pass the last checkpoint
            // - we reached the next one
            if ( (currIndex == nextCheckpoint) && (currCheckpointIndex < obstacleCheckpoints.Count)){
 
                //reached the next checkpoint: update current checkpoint index and checkpoint
                ++currCheckpointIndex;
                currCheckpoint = obstacleCheckpoints[currCheckpointIndex];

                //if possible, look forward to the new nextCheckpoint
                if ( (currCheckpointIndex + 1) < obstacleCheckpoints.Count){
                    nextCheckpoint = obstacleCheckpoints[currCheckpointIndex + 1];
                }
            }

            //get current cutscene
            CutsceneInfo next = cutsceneList[currIndex];
            Debug.Log("CM: current cutscene index = " + currIndex);
            
            //increment currIndex so we see the next Cutscene next time this triggers
            ++currIndex;

            //check if we reached the end of the cutscenes (set up for next time)
            if(currIndex >= cutsceneList.Count){
                ///Debug.Log("CM: this was the last Cutscene: reached the end");
                finishedCutscenes = true;
            }

            return next;
        }
        //if there aren't any cutscenes left, return null
        return null;
    }


    //made these in case CutsceneController needed them (currently it doesn't)
    public void setFinished(bool finished){
        finishedCutscenes = finished;
    }

    public bool isFinished(){
        return finishedCutscenes;
    }


    //GameManager can use these to see what cutscene and checkpoint we passed
    //  (use to decide which obstacles to spawn)
    public int getCurrentCheckpoint(){
        return currCheckpoint;
    }

    public int getCurrentCutscene(){
        return currIndex;
    }

}
