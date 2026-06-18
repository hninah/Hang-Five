using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneCEO : MonoBehaviour
{
    //public variables
    public Vector3 finalPosition;
    public float moveSpeed;

    //private variables
    private Animator animator;
    private bool isMoving = false;


    // Start is called before the first frame update
    void Start(){
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update(){
        if(!isMoving && isPlayingClip(animator, "holdenWalking")){
            isMoving = true;
        }
        
        //if not at final position, move towards it
        if (isMoving && (transform.position - finalPosition).magnitude > 0.05f){
            transform.position += (finalPosition 
                  - transform.position).normalized*Time.deltaTime*moveSpeed;
        }
        //when CEO is far enough offscreen, go to tutorial
        /// (very hardcoded, but works for now)
        else if ((transform.position - finalPosition).magnitude <= 0.05f){
            SceneManager.LoadScene("Tutorial");
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
