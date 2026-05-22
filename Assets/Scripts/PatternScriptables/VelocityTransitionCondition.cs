using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VelocityTransitionType
{
    GTE,
    GT,
    LTE,
    LT
}

public enum VelocityCheckType
{
    ABS,
    REAL
}

[CreateAssetMenu(fileName = "New Velocity Transition", menuName = "Velocity Transition Condition")]
public class VelocityTransitionCondition : PatternTransitionCondition
{
    public float targetVelocity;
    public VelocityTransitionType conditionType;
    public VelocityCheckType velocityCheckType;

    public override bool satisfied(Player player)
    {
        float velocityToCheck = player.Velocity.y * player.AngleSpeedPercentage;

        if (velocityCheckType == VelocityCheckType.ABS)
        {
            velocityToCheck = Mathf.Abs(velocityToCheck);
        }

        switch(conditionType)
        {
            case VelocityTransitionType.GTE:
                return velocityToCheck >= targetVelocity;

            case VelocityTransitionType.LTE:
                return velocityToCheck <= targetVelocity;

            case VelocityTransitionType.LT:
                return velocityToCheck < targetVelocity;

            case VelocityTransitionType.GT:
                return velocityToCheck > targetVelocity;
        }

        return false;
    }
}
