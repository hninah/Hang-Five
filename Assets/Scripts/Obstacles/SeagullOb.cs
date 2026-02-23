using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullOb : Obstacle
{
    ///public float fallSpeed = 2f;
    public float spawnWait = 20f;
    public GameObject obsPrefab;

    //constructors
    public SeagullOb(float scrollSpeed):base("Seagull", scrollSpeed) {}
    public SeagullOb():base("Seagull") {}


    //Start is called before the first frame update
    void Start(){
        //set starting state
        //seagull starts and stays in spawning state
        activeState = new SpawningState(spawnWait, obsPrefab);
        
        ///activeState = new StationaryState(); ///stationary if we don't want it spawning

        activeState.onEnterState();
    }

}
