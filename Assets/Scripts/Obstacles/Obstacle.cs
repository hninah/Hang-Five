using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    //basic obstacle variables
    public float scrollSpeed = 8f;

    private string obsName;
    public string Name { get{ return obsName; } }

    public State activeState; //current state

    //constructors
    public Obstacle( string name, float speed){
        scrollSpeed = speed;
        obsName = name;
    }

    public Obstacle( string name){
        obsName = name;
    }


    //update
    public void Update(){
        scrollMotion();
        activeState.stateUpdate(this);
        nextState();
    }


    //move obstacle left across the screen
    public void scrollMotion(){
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        //destroy the obstacle when offscreen
        if (transform.position.x < -20f){
            Destroy(gameObject);
        }
    }


    //run state transition logic
    public void nextState(){
        State nextState = getNextState();

        if (nextState.Name != activeState.Name){
            ///Debug.Log("moving to " + nextState.Name);

            activeState.onExitState(); //exit old state
            activeState = nextState;
            activeState.onEnterState(); //enter new state
        }
    }


    //child obstacles implement their own state transition logic
    public virtual State getNextState() {
        return activeState;
    }

}
