using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsFg : Obstacle
{
    public CloudsFg(float scrollSpeed) : base("CloudsFg", scrollSpeed) { }
    public CloudsFg() : base("CloudsFg") { }

    void Start()
    {
        activeState = new LoopState();
        activeState.onEnterState();
    }
}
