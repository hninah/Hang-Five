using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatternTransitionCondition : ScriptableObject
{
    public virtual bool satisfied(Player player)
    {
        return false;
    }
}
