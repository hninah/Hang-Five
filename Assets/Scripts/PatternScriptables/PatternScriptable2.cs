using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ObstaclePatternInfo
{
    public GameObject obstacle;
    public int spawnIdx;
    public float timeTillSpawn;
    public bool randomSpawnY;
}

[CreateAssetMenu(fileName="New Pattern2", menuName="Pattern2")]
public class PatternScriptable2 : ScriptableObject
{
    public ObstaclePatternInfo[] obstacleInfoArr;
}
