using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    [Header("Use For Random Speed Range")]
    public float minSpeed = 0.3f;
    public float maxSpeed = 0.8f;

    [Header("Use For Uniform Speed")]
    public bool uniformSpeed;
    public float defaultSpeed = 0.2f;

    //private variables
    private float scrollSpeed;
    private float halfWidth;
    private float halfSpriteWidth;

    //Start is called before the first frame update
    void Start(){
        //get reference width of camera
        Camera camera = Camera.main;
        halfWidth = camera.orthographicSize * camera.aspect;
        
        //get reference width of sprite
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        halfSpriteWidth = (spriteRenderer.bounds.size.x)/2f;
        
        //choose a scroll speed
        if (uniformSpeed){
            scrollSpeed = defaultSpeed;
        }
        //otherwise choose a random scroll speed
        else{
            scrollSpeed = Random.Range(minSpeed, maxSpeed);
        }
    }

    //Update is called once per frame
    void Update(){
        //scroll left
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        //jump to the right if we're off camera
        if ( transform.position.x <= (-halfWidth - halfSpriteWidth) ){

            transform.position += Vector3.right * (halfWidth + halfSpriteWidth) * 2f;

            //get a new random scroll speed if not uniform speed
            if (!uniformSpeed){
                scrollSpeed = Random.Range(minSpeed, maxSpeed);
            }
        }
    }
}
