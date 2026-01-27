using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveScrolling : MonoBehaviour
{
    public float speed = 6f;
    public float segmentWidth = 30f;

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= -segmentWidth)
        {
            transform.position += Vector3.right * segmentWidth * 2f;
        }
    }
}
