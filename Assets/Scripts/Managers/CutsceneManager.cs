using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{

    public static CutsceneManager Instance;

    //check here after wipeout to get the next cutscene
    [Tooltip("All the CutsceneInfos in the order they appear.")]
    public List<CutsceneInfo> cutsceneList;

    //track which cutscene we're on
    private int currIndex;
    //track whether all cutscenes were seen yet
    private bool finishedCutscenes;

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


    public CutsceneInfo getNextCutscene(){
        //return next cutscene if there is one, update checkpoints
        if(!finishedCutscenes){
            
            //get current cutscene
            CutsceneInfo next = cutsceneList[currIndex];
            //increment currIndex so we see the next Cutscene next time this triggers
            ++currIndex;

            //check if we reached the end of the cutscenes (set up for next time)
            if(currIndex >= cutsceneList.Count){
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

    //used in GameManager and PatternManager
    public int getCurrentCutscene(){
        return currIndex;
    }

    //used to reset when we press the Quit button on wipeout screen
    public void resetCutscenes(){
        currIndex = 0;
        finishedCutscenes = false;
    }

}
