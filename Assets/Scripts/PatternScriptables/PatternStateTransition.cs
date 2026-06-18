using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New State Transition", menuName = "State Transition")]
public class PatternStateTransition : ScriptableObject
{
    // The State transitioned to if transitionCondition is true.
    public PatternState patternState;

    // I put the state transition cooldowns here so you have the option
    // to customize how much time you want between different states.
    public float coolDownTime;

    public PatternTransitionCondition[] transitionConditions;

    public virtual bool stateTransition(Player player)
    {
        foreach (PatternTransitionCondition condition in transitionConditions)
        {
            if (!condition.satisfied(player))
            {
                return false;
            }
        }

        return true;
    }

    public virtual PatternState getNextState()
    {
        return patternState;
    }
}
