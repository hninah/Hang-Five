using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Random Transition", menuName="Random Transition Condition")]
public class RandomTransitionCondition : PatternTransitionCondition
{
    [Tooltip("The random number must be greater than or equal to this probability for this condition to be true.")]
    public float minProbabilityInclusive = 0.0f;
    [Tooltip("The random number must be less than this probability for this condition to be true.")]
    public float maxProbabilityExclusive = 1.0f;

    public override bool satisfied(Player player)
    {
        return PatternStateManager.Instance.RngNum >= minProbabilityInclusive && PatternStateManager.Instance.RngNum < maxProbabilityExclusive;
    }
}
