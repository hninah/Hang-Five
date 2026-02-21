using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingOb : Obstacle
{
    public float fallSpeed = 2f;
    
    //constructors
    public FallingOb(float scrollSpeed):base("Falling", scrollSpeed) {}
    public FallingOb():base("Falling") {}


    //Start is called before the first frame update
    void Start(){
        //falling obstacle starts/stays in falling state
        activeState = new FallingState(fallSpeed);
        activeState.onEnterState();
    }

}
