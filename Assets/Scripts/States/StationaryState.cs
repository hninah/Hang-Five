using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryState : State
{
    public float speed = 0f;

    //constructors
    public StationaryState():base("StationaryState"){}
    public StationaryState(float s):base("StationaryState"){
        speed = s;
    }

    public override State stateUpdate(Obstacle ob){
        
        //take the default move speed if nothing specified
        if (speed == 0f){
            speed = ob.scrollSpeed;
        }
        //scroll left across the screen
        ob.transform.position += Vector3.left * speed * Time.deltaTime;

        if (ob.transform.position.x <= ob.deathBoundX)
        {
            return new DeathState();
        }

        return this;
    }

}
