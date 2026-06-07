using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleExpand : MonoBehaviour
{
    public float expandRate = 0.5f;
    public float moveRate = 0.5f;
    private SpriteRenderer spriteRenderer;
    private float maxSize;

    // Start is called before the first frame update
    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        Camera camera = Camera.main;
        maxSize = camera.orthographicSize * camera.aspect * 4.5f;
    }


    // Update is called once per frame
    void Update(){
        transform.position += new Vector3(moveRate, -moveRate, 0f)*Time.deltaTime;
        transform.localScale += new Vector3(expandRate, expandRate, 0f)*Time.deltaTime;

        if (transform.localScale.x >= maxSize){
            gameObject.active = false;
        }
    }
}
