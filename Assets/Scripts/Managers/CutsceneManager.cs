using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{

    public static CutsceneManager Instance;

    //check here after wipeout to get the next cutscene
    public List<CutsceneInfo> cutsceneList;

    //track which cutscene we're on
    private int currIndex;
    //track whether all cutscenes were seen yet
    private bool finishedCutscenes;

    private void Awake()
    {
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);

            //start game on the first cutscene
            currIndex = 0;
            //check starting list length to see if we finished or not
            if (cutsceneList.Count > 0) finishedCutscenes = false;
            else finishedCutscenes = true;
        }
        else{
            Destroy(gameObject);
        }
    }


    public CutsceneInfo getNextCutscene(){
        //return next cutscene if there is one
        if(!finishedCutscenes){
            //get current cutscene and increment currIndex
            CutsceneInfo next = cutsceneList[currIndex++];

            //after incrementing, check if we reached the end
            // (setup for next time)
            if(currIndex >= cutsceneList.Count){
                finishedCutscenes = true;
            }

            return next;
        }
        //if there aren't any cutscenes left, return null
        return null;
    }


    public int getCurrCutsceneIndex(){
        return currIndex;
    }

    public void setFinished(bool finished){
        finishedCutscenes = finished;
    }

    public bool isFinished(){
        return finishedCutscenes;
    }

}
