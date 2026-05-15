using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State Transition", menuName = "State Transition Condition")]
public class StateTransitionCondition : PatternTransitionCondition
{
    public Player.PlayerState desiredState;

    public override bool satisfied(Player player)
    {
        return player.State == desiredState;
    }
}
