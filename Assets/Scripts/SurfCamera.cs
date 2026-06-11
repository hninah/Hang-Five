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

    private bool screenShaking = false;
    public bool ScreenShaking { get { return screenShaking; } set { screenShaking = value; } }
    public float shakeOffsetX = 0.3f;
    public float shakeOffsetY = 0.3f;

    //camera shake version for target score effect
    public float shakeFXTime = 0.5f;
    private bool isShakeFX = false;
    public bool IsShakeFX { get { return isShakeFX; } set { isShakeFX = value; } }
    private float timer = 0f;

    void Start()
    {
        cameraIdlePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (screenShaking)
        {
            transform.position = cameraIdlePosition + new Vector3(Random.Range(0.0f, shakeOffsetX), Random.Range(0.0f, shakeOffsetY), 0.0f);
            
            //set a timer for the target score shake effect
            if (isShakeFX && timer < shakeFXTime){
                timer += Time.deltaTime;
                Debug.Log("timer = " + timer);
            }
            //stop shaking when timer ends
            if(timer >= shakeFXTime){
                screenShaking = false;
                timer = 0f;
                isShakeFX = false;
                Debug.Log("stopped shaking");
            }
            return;
        }

        // FIXME: This follow code is probably terrible (might be better to scale the camera or do something more complicated than this)
        if (player.State == Player.PlayerState.FLIPPING)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, player.transform.position.y, transform.position.z) + new Vector3(0.0f, surfBoardOffsetY, 0.0f), cameraSpeed * Time.deltaTime);
        }
        else if (player.State == Player.PlayerState.SURFING && Mathf.Abs(cameraIdlePosition.y - transform.position.y) >= 0.00001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, cameraIdlePosition, cameraSpeed * Time.deltaTime);
        }
    }

    public void ResetCamera()
    {
        // resetting the camera for tutorial
        transform.position = cameraIdlePosition;
    }
}
