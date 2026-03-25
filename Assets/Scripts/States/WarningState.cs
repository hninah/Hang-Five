using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningState : State
{
    private float storedScrollSpeed;
    private Animator animator;

    //constructors
    public WarningState():base("WarningState"){}

    public WarningState(float scrollSpeed):
        base("WarningState")
    {
        storedScrollSpeed = scrollSpeed;
    }


    public override void onEnterState(Obstacle ob){
        ob.scrollSpeed = 0f;

        animator = ob.GetComponent<Animator>();
        if(animator != null){
            ob.GetComponent<Animator>().SetBool("isWarningEnded", false);
        }
    }


    public override void onExitState(Obstacle ob){
        ob.scrollSpeed = storedScrollSpeed;

        if(animator != null){
            ob.GetComponent<Animator>().SetBool("isWarningEnded", true);
        }
    }

}
