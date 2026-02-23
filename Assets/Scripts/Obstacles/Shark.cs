using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : Obstacle
{

    public Shark(float scrollSpeed) : base("Shark", scrollSpeed) { }
    public Shark() : base("Shark") { }

    public float aggroThreshold = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        activeState = new SharkPassiveState();
        activeState.onEnterState();
    }

    public override void obstacleSpecialties()
    {
        if (Mathf.Abs(transform.position.x - Player.Instance.transform.position.x) < aggroThreshold)
        {
            animator.SetBool("isAttacking", true);
        }
    }
}
