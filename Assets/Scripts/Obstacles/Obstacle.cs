using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    //basic obstacle variables
    [Header("Obstacle Parameters")]
    public float scrollSpeed = 5f;
    public float maxYBound = 0.0f;
    public float minYBound = 0.0f;
    public float deathBoundX = -12.0f;

    [Header("Use Obstacle After This Cutscene")]
    [Tooltip("The cutscene after which this obstacle first appears (0-indexed, so if this equals 0, it will appear AFTER the first cutscene and if -1, it will appear BEFORE the first cutscene).")]
    [SerializeField] private int checkpointCutscene = 0; //obstacle should appear after this cutscene

    //obstacle name for debugging
    private string obsName;
    public string Name { get{ return obsName; } }

    public State activeState; //current state
    public Animator animator;

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
        activeState = activeState.stateUpdate(this);

        if (activeState.Name == "DeathState")
        {
            Destroy(gameObject);
        }

        obstacleSpecialties();
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

            activeState.onExitState(this); //exit old state
            activeState = nextState;
            activeState.onEnterState(this); //enter new state
        }
    }


    //child obstacles implement their own state transition logic
    public virtual State getNextState() {
        return activeState;
    }

    // For dynamically setting the boundaries an obstacle should never move out of
    public void setYBounds(float minY, float maxY)
    {
        minYBound = minY;
        maxYBound = maxY;
    }

    public virtual void obstacleSpecialties()
    {
        return;
    }

    public int getCheckpointCutscene()
    {
        return checkpointCutscene;
    }
}
