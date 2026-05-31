using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum YTransitionType
{
    GTE,
    LTE,
    LT,
    GT
}

public enum YTransitionLogical
{
    AND,
    OR,
    IMMEDIATE_TRUE
}

[System.Serializable]
public struct YConditionType
{
    public YTransitionType conditionType;
    public float yCoordinate;
    public YTransitionLogical logicType;
}

[CreateAssetMenu(fileName = "New Y Transition", menuName = "Y Transition Condition")]
public class YTransition : PatternTransitionCondition
{
    public YConditionType[] yConditions;

    public override bool satisfied(Player player)
    {
        bool orSatisfied = false;

        foreach (YConditionType condition in yConditions)
        {
            bool inBounds = inYBounds(player, condition);

            // If any of the conditions are satisifed, this will pass
            if (!inBounds && condition.logicType == YTransitionLogical.AND)
            {
                return false;
            }
            else if (inBounds && condition.logicType == YTransitionLogical.IMMEDIATE_TRUE)
            {
                return true;
            }
            else if (inBounds)
            {
                orSatisfied = true;
            }
        }

        return orSatisfied;
    }

    public bool inYBounds(Player player, YConditionType condition)
    {
        switch(condition.conditionType)
        {
            case YTransitionType.GTE:
                return player.transform.position.y >= condition.yCoordinate;

            case YTransitionType.LTE:
                return player.transform.position.y <= condition.yCoordinate;

            case YTransitionType.LT:
                return player.transform.position.y < condition.yCoordinate;

            case YTransitionType.GT:
                return player.transform.position.y > condition.yCoordinate;
        }

        // This should never happen, but I get a compiler warning if this isn't here.
        return false;
    }
}
