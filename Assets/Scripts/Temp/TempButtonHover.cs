using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempButtonHover : MonoBehaviour
{
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time;
        float y = Mathf.Sin(t * 4f) * 2f;
        transform.localPosition = startPos + new Vector3(0, y, 0);
    }
}
