using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : State
{
    //vertical falling speed
    float fallSpeed = 2f;

    //constructors
    public FallingState():base("FallingState"){}
    public FallingState(float fall):base("FallingState"){
        fallSpeed = fall;
    }


    //update for this state
    public override void stateUpdate(Obstacle ob){

        //fall downwards
        ob.transform.position -= new Vector3(0f, fallSpeed*Time.deltaTime, 0f);
    }


    public override void onEnterState(Obstacle ob){
        ///Debug.Log("entered " + this.Name);
    }

    public override void onExitState(Obstacle ob){
        ///Debug.Log("exited " + this.Name);
    }
}
