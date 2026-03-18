using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPattern : Pattern
{
    //additional public variables
    [Header("Vertical Spacing of Wall Obstacles")]
    public float spacing = 2f;

    //private variables
    private float latestY;
    private bool controllingTimer = true; //pause the spawn timer to make a wall

    /*
    //constructors
    public WallPattern(float minY, float maxY) : 
        base("WallPattern", minY, maxY)
    {
        latestY = minY;
    }

    public WallPattern(float minY, float maxY, float s) : 
        base("WallPattern", minY, maxY)
    {
        latestY = minY;
        spacing = s;
    }
    */


    //get Y Position for new obstacles in the pattern
    public override float patternSpawnY(){
        //take over timer to make the wall
        controllingTimer = true;

        //get current wall spot, update for next time
        float currentYPos = latestY;
        latestY += spacing;

        //reached the end of the wall
        if( latestY > maxSpawnY ){
            latestY = minSpawnY;
            //return timer control to lane spawner
            controllingTimer = false;
        }
        return currentYPos;
    }


    //functions used to give LaneSpawner.cs info about the timer status
    public override bool isTimerPaused(){
        //normal timer is paused when pattern controls the timer
        if( controllingTimer ) return true;
        return false;
    }

}
