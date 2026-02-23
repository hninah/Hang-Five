using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopState : State
{
    [SerializeField] float loopEndX = -20.0f;
    [SerializeField] float loopStartX = 21.0f;

    public LoopState() : base("LoopState") { }
    public LoopState(float startX, float endX) : base("LoopState")
    {
        loopEndX = endX;
        loopStartX = startX;
    }

    //update for this state
    public override State stateUpdate(Obstacle ob)
    {
        ob.transform.position = ob.transform.position + Vector3.left * ob.scrollSpeed * Time.deltaTime;

        if (ob.transform.position.x < loopEndX)
        {
            ob.transform.position = new Vector3(loopStartX, ob.transform.position.y, ob.transform.position.z);
        }

        return this;
    }
}
