using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningState : State
{
    //time between dropping an obstacle
    public float spawnWait = 20f;

    //time since last obstacle spawn
    private float currWait = 0f;
    private GameObject obsPrefab;

    //constructors
    public SpawningState():base("SpawningState") {}
    public SpawningState(float wait):base("SpawningState"){
        spawnWait = wait;
    }
    public SpawningState(GameObject prefab):base("SpawningState"){
        obsPrefab = prefab;
    }
    public SpawningState(float wait, GameObject prefab):base("SpawningState"){
        spawnWait = wait;
        obsPrefab = prefab;
    }


    //update for this state
    public override State stateUpdate(Obstacle ob){

        ///Debug.Log("currWait = " + currWait + ", spawnWait = " + spawnWait);

        //spawn obstacle if finished waiting
        if ( currWait >= spawnWait ){
            ///Debug.Log("currWait = " + currWait + ", spawnWait = " + spawnWait + ": spawning a new faller");
            Object.Instantiate(obsPrefab, ob.transform.position, Quaternion.identity);
            currWait = 0f;
        }
        else{
            currWait += Time.deltaTime;
        }

        return this;
    }


    public override void onEnterState(Obstacle ob){
        ///Debug.Log("entered " + this.Name);
    }

    public override void onExitState(Obstacle ob){
        ///Debug.Log("exited " + this.Name);
    }

}
