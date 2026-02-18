using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAggroState : State
{
    Vector3 targetDirection = new Vector3(-1.0f, 0.0f, 0.0f);
    [SerializeField] float aggroSpeed = 12.0f;

    public SharkAggroState() : base("SharkPassiveState") { }
    public SharkAggroState(Vector3 currentPosition, Vector3 targetPosition) : base("SharkAggroState")
    {
        float directionAngle = Mathf.Atan2(targetPosition.y - currentPosition.y, targetPosition.x - currentPosition.x);
        targetDirection.x = Mathf.Cos(directionAngle);
        targetDirection.y = Mathf.Sin(directionAngle);
    }

    public override State stateUpdate(Obstacle ob)
    {
        Vector3 newPosition = ob.transform.position + targetDirection * aggroSpeed * Time.deltaTime;
        newPosition.y = Mathf.Clamp(newPosition.y, ob.minYBound, ob.maxYBound);

        ob.transform.position = newPosition;

        if (newPosition.x < ob.deathBoundX)
        {
            return new DeathState();
        }

        return this;
    }

}
