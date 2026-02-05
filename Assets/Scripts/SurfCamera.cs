using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfCamera : MonoBehaviour
{
    [Tooltip("Offset of the camera from the player in the y direction.")]
    public float surfBoardOffsetY;
    [Tooltip("Speed the camera follows the player at.")]
    public float cameraSpeed;
    [Tooltip("Player the camera should follow if necessary.")]
    public Player player;
    [Tooltip("Where the camera should normally be during gameplay.")]
    public Vector3 cameraIdlePosition;

    // Update is called once per frame
    void Update()
    {
        // FIXME: This follow code is probably terrible (might be better to scale the camera or do something more complicated than this)
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
