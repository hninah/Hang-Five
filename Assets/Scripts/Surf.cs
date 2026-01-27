using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surf : MonoBehaviour
{
    public Transform[] laneAnchors;
    public int currentLane = 1;     
    public float laneChangeSpeed = 12f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            currentLane = Mathf.Clamp(currentLane - 1, 0, laneAnchors.Length - 1);

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            currentLane = Mathf.Clamp(currentLane + 1, 0, laneAnchors.Length - 1);

        Vector3 pos = transform.position;
        float targetY = laneAnchors[currentLane].position.y;
        pos.y = Mathf.MoveTowards(pos.y, targetY, laneChangeSpeed * Time.deltaTime);
        transform.position = pos;
    }
}
