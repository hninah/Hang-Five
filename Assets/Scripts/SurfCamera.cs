using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfCamera : MonoBehaviour
{
    public float surfBoardOffsetY;
    public float cameraSpeed;
    public Player player;
    public Vector3 cameraIdlePosition;

    // Update is called once per frame
    void Update()
    {
        if (player.state == Player.PlayerState.FLIPPING)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, player.transform.position.y, transform.position.z) + new Vector3(0.0f, surfBoardOffsetY, 0.0f), cameraSpeed * Time.deltaTime);
        }
        else if (player.state == Player.PlayerState.SURFING && Mathf.Abs(cameraIdlePosition.y - transform.position.y) >= 0.00001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, cameraIdlePosition, cameraSpeed * Time.deltaTime);
        }
    }
}
