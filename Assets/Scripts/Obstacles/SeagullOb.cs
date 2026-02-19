using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullOb : Obstacle
{
    //constructors
    public SeagullOb(float scrollSpeed):base("Seagull", scrollSpeed) {}
    public SeagullOb():base("Seagull") {}


    //Start is called before the first frame update
    void Start(){
        //set starting state
        //seagull starts in stationary state
        activeState = new StationaryState();
        activeState.onEnterState();
    }


    public override State getNextState(){
        //logic to switch between states
        
        /*
        if (transform.position.x < 0f && activeState.Name != "StationaryState"){
            return new StationaryState();
        }
        */
        //default: stay in current state
        return activeState;
    }
}
