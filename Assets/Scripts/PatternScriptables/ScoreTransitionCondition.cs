using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScoreTransitionType
{
    GTE,
    LTE
}

[CreateAssetMenu(fileName = "New Score Transition", menuName = "Score Transition Condition")]
public class ScoreTransitionCondition : PatternTransitionCondition
{
    float scoreThreshold;
    ScoreTransitionType condition;

    public override bool satisfied(Player player)
    {
        switch(condition)
        {
            case ScoreTransitionType.GTE:
                return ScoreManager.Instance.score >= scoreThreshold;

            case ScoreTransitionType.LTE:
                return ScoreManager.Instance.score <= scoreThreshold;
        }

        return false;
    }
}
