using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileOb : Obstacle
{
    [Header("Missile Parameters")]
    public Sprite warningSprite;
    public Sprite missileSprite;
    public float warningTimer;

    //constructors
    public MissileOb(float scrollSpeed):base("Missile", scrollSpeed) {}
    public MissileOb():base("Missile") {}


    void Start(){
        activeState = new WarningState(scrollSpeed, warningSprite, missileSprite);
        activeState.onEnterState(this);
    }


    public override State getNextState(){
        warningTimer -= Time.deltaTime;

        //when warning's over, go to basic movement
        if (warningTimer <= 0f){
            return new StationaryState(scrollSpeed);
        }
        
        //destroy object if it's out of bounds
        if (transform.position.x <= deathBoundX){
            return new DeathState();
        }

        return activeState;
    }

}
