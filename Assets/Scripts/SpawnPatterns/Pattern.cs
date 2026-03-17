using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern: MonoBehaviour
{
    //spawning locations
    [Header("Spawning Locations")]
    public float minSpawnY = 0f;
    public float maxSpawnY = 0f;
    public float spawnX = 12f;
    
    [Header("Use Pattern After This Cutscene")]
    [Tooltip("The cutscene after which this pattern first appears (0-indexed, so if this equals 0, it will appear AFTER the first cutscene and if -1, it will appear BEFORE the first cutscene).")]
    [SerializeField] private int checkpointCutscene = 0; //pattern can be used after this cutscene

    [Header("Pattern Name For Debugging")]
    [SerializeField] private string patternName;
    public string Name { get{ return patternName; } }

    /*
    public Pattern(string name, float min, float max){
        patternName = name;
        minSpawnY = min;
        maxSpawnY = max;
    }

    public Pattern(string name, float min, float max, float x){
        patternName = name;
        minSpawnY = min;
        maxSpawnY = max;
        spawnX = x;
    }
    */

    public virtual Vector3 patternSpawnPos(){ return new Vector3(0, 0, 0); }
    public virtual float patternSpawnY(){ return 0; }
    public virtual bool shouldSpawn(){ return true; }

    public virtual bool isTimerPaused(){ return false; }
    public virtual float getTimer(){ return 0; }

    public int getCheckpointCutscene(){ 
        return checkpointCutscene;
    }

}
