using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkPassiveState : State
{
    [SerializeField] float aggroDistanceX = 8.5f;
    [SerializeField] float frequency = 3.0f;
    [SerializeField] float amplitude = 0.08f;

    //constructors
    public SharkPassiveState() : base("SharkPassiveState") { }
    public SharkPassiveState(float freq, float amp) : base("SharkPassiveState")
    {
        frequency = freq;
        amplitude = amp;
    }

    //update for this state
    public override State stateUpdate(Obstacle ob)
    {
        Vector3 targetPosition = Player.Instance.transform.position;

        State newState = this;

        Vector3 newPosition = ob.transform.position + Vector3.left * ob.scrollSpeed * Time.deltaTime;

        //add wave offset to position
        Vector3 waveOffset = Vector3.up * Mathf.Sin(Time.time * frequency) * amplitude;
        newPosition += waveOffset * Time.deltaTime;
        newPosition.y = Mathf.Clamp(newPosition.y, ob.minYBound, ob.maxYBound);

        ob.transform.position = newPosition;

        if (Mathf.Abs(ob.transform.position.x - targetPosition.x) < aggroDistanceX
            && ob.transform.position.x > targetPosition.x)
        {
            onExitState();
            newState = new SharkAggroState(ob.transform.position, Player.Instance.transform.position);
            newState.onEnterState();
        }
        else if (ob.transform.position.x < ob.deathBoundX)
        {
            onExitState();
            newState = new DeathState();
            newState.onEnterState();
        }

        return newState;
    }


}
