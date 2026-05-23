using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Pattern State", menuName="Pattern State")]
public class PatternState : ScriptableObject
{
    public PatternScriptable2 currentPattern;
    public PatternStateTransition[] transitions;
    public float conditionCheckTimer;
}
