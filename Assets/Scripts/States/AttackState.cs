using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    //how far from player the shark should attack
    float attackDistance = 3f;
    //how fast the shark should attack
    float attackSpeed = 1f;
    //transform to track player position
    Transform player;

    //constructors
    public AttackState():base("AttackState") {}
    public AttackState(float dist, float speed):base("AttackState"){
        attackDistance = dist;
        attackSpeed = speed;
    }
    public AttackState(Transform p):base("AttackState"){
        player = p;
    }
    public AttackState(float dist, float speed, Transform p):base("AttackState"){
        attackDistance = dist;
        attackSpeed = speed;
        player = p;
    }


    //update for this state
    public override void stateUpdate(Obstacle ob){

        //move towards the player
        ob.transform.position = Vector3.MoveTowards( 
            ob.transform.position, 
            player.position,
            attackSpeed * Time.deltaTime
        );
    }


    //modify animation when we enter or leave this state
    public override void onEnterState(Obstacle ob){
        Debug.Log("entered " + this.Name);
        ob.animator.SetBool("isAttacking", true);
    }

    public override void onExitState(Obstacle ob){
        Debug.Log("exited " + this.Name);
        ob.animator.SetBool("isAttacking", false);
    }
}
