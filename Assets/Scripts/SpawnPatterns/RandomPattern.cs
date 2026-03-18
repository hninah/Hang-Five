using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPattern : Pattern
{   
    
    //get Y Position for new obstacles in the pattern
    public override float patternSpawnY(){
        //random start position
        return Random.Range(minSpawnY, maxSpawnY);
    }

}
