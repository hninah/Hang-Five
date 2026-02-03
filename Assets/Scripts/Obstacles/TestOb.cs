using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOb : Obstacle
{
    public float frequency = 3f;
    public float amplitude = 0.08f;

    public TestOb(float scrollSpeed):base("Test", scrollSpeed) {}
    public TestOb():base("Test") {}


    void Start(){
        //set starting state
        activeState = new SineWaveState(frequency, amplitude);
        activeState.onEnterState();
    }

    
    public override State getNextState(){
        //logic to switch between states
        if (transform.position.x < 0f && activeState.Name != "StationaryState"){
            return new StationaryState();
        }
        //default: stay in current state
        return activeState;
    }

}
