using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Transition", menuName = "Default Transition Condition")]
public class DefaultTransitionCondition : PatternTransitionCondition
{
    public override bool satisfied(Player player)
    {
        return true;
    }
}
