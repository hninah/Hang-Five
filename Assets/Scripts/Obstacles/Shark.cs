using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : Obstacle
{

    public Shark(float scrollSpeed) : base("Shark", scrollSpeed) { }
    public Shark() : base("Shark") { }

    // Start is called before the first frame update
    void Start()
    {
        activeState = new SharkPassiveState();
        activeState.onEnterState();
    }
}
