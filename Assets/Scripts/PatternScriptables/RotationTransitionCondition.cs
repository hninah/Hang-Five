using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationTransitionType
{
    GTE,
    LTE
}

[CreateAssetMenu(fileName="New Rotation Transition", menuName="Rotation Transition Condition")]
public class RotationTransitionCondition : PatternTransitionCondition
{
    public float rotationThreshold;
    public RotationTransitionType condition;

    public override bool satisfied(Player player)
    {
        switch(condition)
        {
            case RotationTransitionType.GTE:
                return player.Rotation >= rotationThreshold;

            case RotationTransitionType.LTE:
                return player.Rotation <= rotationThreshold;
        }

        return false;
    }
}
