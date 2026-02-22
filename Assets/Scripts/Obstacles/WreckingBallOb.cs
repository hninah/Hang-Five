using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBallOb : Obstacle
{
    //constructors
    public WreckingBallOb(float scrollSpeed):base("WreckingBall", scrollSpeed) {}
    public WreckingBallOb():base("WreckingBall") {}


    //Start is called before the first frame update
    void Start(){
        //set starting state
        activeState = new StationaryState();
        activeState.onEnterState();
    }
}
