using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WedgePattern : Pattern
{
    //additional public variables
    [Header("Wedge Parameters")]
    public float spacingY = 1f; //vertical spacing
    public float spacingX = 1f; //horizontal spacing between layers
    public float numLayers = 3; //number of layers in the wedge

    [Header("Time Between Wedge Patterns")]
    public float gapTime = 3f;

    //private variables
    private float centreY; //starting y position for the wedge
    private float latestY;
    private float currLayer = 0;

    //skip spawning an obstacle if it would be out of bounds
    private bool shouldSpawnObstacle = true;

    private bool controllingTimer = true; //pause spawn timer to make a wedge
    private float controlledTimer;

    //get Y Position for new obstacles in the pattern
    public override float patternSpawnY(){
        //take over the timer to make this pattern
        controllingTimer = true;
        //reset spawn controller
        shouldSpawnObstacle = true;

        //place the first point
        if (currLayer == 0){
            //reset to a new random centre
            centreY = Random.Range(minSpawnY, maxSpawnY);
            latestY = centreY;
            controlledTimer = spacingX;
            ++currLayer;
            shouldSpawnObstacle = true;
        }
        //place the next layer
        else if (currLayer < numLayers){

            //do the top
            if ( latestY <= centreY ){
                latestY = centreY + spacingY * currLayer;
                controlledTimer = 0f;

                //don't spawn if out of bounds
                if (latestY > maxSpawnY){ 
                    shouldSpawnObstacle = false; 
                }
            }

            //do the bottom if we already did the top
            else if (latestY > centreY){
                latestY = centreY - spacingY * currLayer;
                //done this layer, so reset timer
                controlledTimer = spacingX;
                ++currLayer;

                //don't spawn if out of bounds
                if (latestY < minSpawnY){ 
                    shouldSpawnObstacle = false; 
                }

                //reset if we finished the wedge
                if (currLayer >= numLayers){
                    controlledTimer = gapTime;
                    //reset for next wedge
                    currLayer = 0;
                    shouldSpawnObstacle = true;
                }
            }
        }

        return latestY;
    }


    //functions used to give LaneSpawner info about the timer status
    public override bool isTimerPaused(){
        //paused when pattern controls the timer
        if( controllingTimer ) return true;
        return false;
    }

    public override float getTimer(){
        return controlledTimer;
    }

    //used to give LaneSpawner info about whether to spawn
    public override bool shouldSpawn(){
        return shouldSpawnObstacle;
    }

}
