using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Endless Transition", menuName = "Endless Transition")]
public class EndlessStateTransition : PatternStateTransition
{
    public PatternState[] transitionStates;

    public override bool stateTransition(Player player)
    {
        return true;
    }

    public override PatternState getNextState()
    {
        return transitionStates[Random.Range(0, transitionStates.Length)];
    }
}
