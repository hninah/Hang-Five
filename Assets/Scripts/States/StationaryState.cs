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

}
