using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningState : State
{
    public Sprite warningSprite;
    public Sprite missileSprite;

    private float storedScrollSpeed;
    private SpriteRenderer spriteRenderer;

    //constructors
    public WarningState():base("WarningState"){}

    public WarningState(float scrollSpeed, Sprite wSprite, Sprite mSprite):
        base("WarningState")
    {
        storedScrollSpeed = scrollSpeed;
        warningSprite = wSprite;
        missileSprite = mSprite;
    }


    public override void onEnterState(Obstacle ob){
        ob.scrollSpeed = 0f;
        ob.GetComponent<SpriteRenderer>().sprite = warningSprite;
    }


    public override void onExitState(Obstacle ob){
        ob.scrollSpeed = storedScrollSpeed;
        ob.GetComponent<SpriteRenderer>().sprite = missileSprite;
    }
}
