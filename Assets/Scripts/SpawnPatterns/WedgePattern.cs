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
    public float gapTime = 6f;

    //private variables
    private float centreY; //starting y position for the wedge
    private float latestY;
    private float currLayer = 0;

    ///skip spawning an obstacle if it would be out of bounds///
    private bool shouldSpawnObstacle = true;

    private bool controllingTimer = true; //pause spawn timer to make a wedge
    private float controlledTimer;

    /*
    //constructors
    public WedgePattern(float minY, float maxY) : 
        base("WedgePattern", minY, maxY)
    {
        Debug.Log("CALLED WEDGE CONSTRUCTOR");
        ///centreY = (int)((maxY + minY)/2); //default centre
        centreY = Random.Range(minSpawnY, maxSpawnY); //start at random centre
        latestY = centreY;
        controlledTimer = spacingX;
    }

    public WedgePattern(float minY, float maxY, float spaceY) : 
        base("WedgePattern", minY, maxY)
    {
        ///centreY = (int)((maxY + minY)/2); //default centre
        centreY = Random.Range(minSpawnY, maxSpawnY); //start at random centre
        latestY = centreY;
        spacingY = spaceY;
        controlledTimer = spacingX;
    }
    */


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
                ///float newTopY = centreY + spacingY * currLayer;
                latestY = centreY + spacingY * currLayer;
                controlledTimer = 0f;

                //don't spawn if out of bounds
                ///if (newTopY <= maxSpawnY){ latestY = newTopY; }
                if (latestY > maxSpawnY){ 
                    Debug.Log("WP: don't spawn because too high");
                    shouldSpawnObstacle = false; 
                }
            }

            //do the bottom if we already did the top
            else if (latestY > centreY){
                ///float newBottomY = centreY - spacingY * currLayer;
                latestY = centreY - spacingY * currLayer;
                //done this layer, so reset timer
                controlledTimer = spacingX;
                ++currLayer;

                //don't spawn if out of bounds
                ///if (newBottomY >= minSpawnY){ latestY = newBottomY; }
                if (latestY < minSpawnY){ 
                    Debug.Log("WP: don't spawn because too low");
                    shouldSpawnObstacle = false; 
                }

                ///reset if we finished the wedge
                ///if (currLayer >= numLayers || (centreY + spacingY * currLayer) > maxSpawnY || (centreY + spacingY * currLayer) < minSpawnY){
                if (currLayer >= numLayers){
                    ///controllingTimer = false;
                    controlledTimer = gapTime;
                    //reset for next wedge
                    currLayer = 0;
                    shouldSpawnObstacle = true;
                }
            }
        }

        return latestY;
    }


    //functions used to give LaneSpawner.cs info about the timer status
    public override bool isTimerPaused(){
        //paused when pattern controls the timer
        if( controllingTimer ) return true;
        return false;
    }

    public override float getTimer(){
        return controlledTimer;
    }

    //functions to give LaneSpawner.cs info about whether to spawn
    public override bool shouldSpawn(){
        return shouldSpawnObstacle;
    }

}
