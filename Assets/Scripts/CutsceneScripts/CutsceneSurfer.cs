using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneSurfer : MonoBehaviour
{   
    [System.Serializable]
    public struct AnimationStep
    {
        //final position of character after this step
        public Vector3 finalPos;

        //speed to move to the final position
        public float moveSpeed;

        //direction change
        public bool flipX;
    }

    //public variables
    [Tooltip("Use to trigger seagull animation")]
    public Animator seagullAnimator;

    [Tooltip("Include One Step Per Clip")]
    public List<AnimationStep> steps;

    //private variables
    private Animator animator;
    private SpriteRenderer spriteRen;
    private int clipIndex = 0;
    private bool isRunning = false;


    // Start is called before the first frame update
    void Start(){
        //set up surfer controllers
        animator = GetComponent<Animator>();
        spriteRen = GetComponent<SpriteRenderer>();
    }


    void Update(){

        //move to next step instructions when second clip starts
        if (!isRunning && isPlayingClip(animator, "uweRunning")){
            //start the seagull animation
            seagullAnimator.SetTrigger("startMoving");

            //move to next clip info
            ++clipIndex;
            isRunning = true;

            //flip sprite if needed
            if (steps[clipIndex].flipX){
                spriteRen.flipX = true;
            }
            else { spriteRen.flipX = false; }
        }

        //if not at final position, move towards it
        if(isRunning && (transform.position - steps[clipIndex].finalPos).magnitude > 0.05f){
            
            transform.position += (steps[clipIndex].finalPos 
                  - transform.position).normalized*Time.deltaTime*steps[clipIndex].moveSpeed;
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
