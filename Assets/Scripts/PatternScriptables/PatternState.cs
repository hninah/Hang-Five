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

[System.Serializable]

[CreateAssetMenu(fileName="New Pattern State", menuName="Pattern State")]
public class PatternState : ScriptableObject
{
    public ObstaclePatternInfo[] obstacleInfoArr;
    public PatternStateTransition[] transitions;
    public float conditionCheckTimer;
}
