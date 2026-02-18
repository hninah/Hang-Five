using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class State
{
    //state name variable
    private string stateName;
    public string Name { get{ return stateName; } }


    //constructor
    public State( string name){
        stateName = name;
    }

    //default: do nothing
    public virtual void stateUpdate(Obstacle ob){ }

    public virtual void onEnterState(Obstacle ob){}
    public virtual void onEnterState(){}

    public virtual void onExitState(Obstacle ob){}
    public virtual void onExitState(){}
}
