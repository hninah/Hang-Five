using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Pattern", menuName="Pattern")]
public class PatternScriptable : ScriptableObject
{

    public GameObject[] obstacles;
    public int[] spawnPointIdxs;
    public float[] timeTillSpawn;
    public int[] checkPointCutscenes;
    public float coolDownTime = 10.0f;
}
