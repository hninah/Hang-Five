using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneSeagulls : MonoBehaviour
{
    //public variables
    public Vector3 finalPosition;
    public float moveSpeed;

    [Tooltip("CEO starts walking when seagulls reach this position")]
    public Vector3 triggerPosition;
    public Animator ceoAnimator;

    //private variables
    private Animator animator;
    private bool isMoving = false;


    // Start is called before the first frame update
    void Start(){
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update(){
        //start moving when the move clip is triggered (see CutsceneSurfer.cs)
        if(!isMoving && isPlayingClip(animator, "cutsceneSeagulls")){
            isMoving = true;
        }
        
        //if not at final position, move towards it
        if (isMoving && (transform.position - finalPosition).magnitude > 0.05f){
            transform.position += (finalPosition 
                  - transform.position).normalized*Time.deltaTime*moveSpeed;
        }

        //start the ceo moving at a certain position
        if (isMoving && (transform.position - triggerPosition).magnitude > 0.05f){
            ceoAnimator.SetTrigger("startMoving");
        }
    }


    //check if animator is playing a given clip by name
    bool isPlayingClip(Animator anim, string animStateName){
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(animStateName) 
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f){
        
            return true;
        }
        else{ return false; }
    }
}
