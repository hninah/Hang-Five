using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryState : State
{
    //constructors
    public StationaryState():base("StationaryState") {}

    //override functions
    public override void onEnterState(){
        ///Debug.Log("entered " + this.Name);
    }

    public override State stateUpdate(Obstacle ob)
    {
        ob.transform.position += Vector3.left * ob.scrollSpeed * Time.deltaTime;

        if (ob.transform.position.x <= ob.deathBoundX)
        {
            return new DeathState();
        }

        return this;
    }

}
