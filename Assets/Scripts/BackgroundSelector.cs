using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSelector : MonoBehaviour
{
    //public variables
    public List<GameObject> backgrounds;

    [Header("Change Background After These Cutscenes")]
    public List<int> cutsceneChanges;

    //private variables
    private int nextChange;

    //Start is called every time we return to Gameplay scene
    void Start(){
        
        //check the cutscene manager for the current cutscene
        int nextCutscene = CutsceneManager.Instance.getCurrentCutscene();
        int changeCount = 0;

        foreach (int checkpoint in cutsceneChanges){
            //count how many changes we've passed
            if ( checkpoint <= nextCutscene ){
                ++changeCount;
            }
        }

        //otherwise enable the correct background group
        for ( int i = 0; i < backgrounds.Count; ++i){
            
            //set active if we've looped through the changes to this one
            if (i == (changeCount - 1) % backgrounds.Count){
                backgrounds[i].SetActive(true);
            }
            //otherwise disable it
            else{
                backgrounds[i].SetActive(false);
            }
        }

    }

}
