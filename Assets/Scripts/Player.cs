using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    /* TODO:
     * - Add a turn transition with weight to it based on how fast the player is moving
     *      - Not quite sure how I would want to do this
     *          - Maybe make rotation the variable input controls, and determine the direction we move in based on rotation (e.g. >= 10 degrees = up and <= 10 degrees = down?)
     *          - Maybe the X rotation on the board needs to be changed too for juice?
     *          - Goal is to get the board to slow down when turning, but maintain some speed into the transition
     * - Add an airborne state (maybe even refactor to a state machine)
     * - Add crashing (board rotates too far in one direction, go beyond certain positions)
     * - Add collision with obstacles
    */
    [Header("Surfing Movement Variables")]
    [SerializeField] private float accel;
    [SerializeField] private float decel;
    [SerializeField] private Vector3 maxSpeed;
    [SerializeField] private Vector3 startingVelocity;

    [Space(20)]
    [Header("Surfing Turning Variables")]
    [SerializeField] private float maxRotation;
    [Tooltip("Temp Variable. Change later with a more viable solution.")]
    [SerializeField] private float magicTurnModifier = -0.25f;
    [SerializeField] private float rotationSpeed;

    // Input handlers
    public PlayerInput playerInput;
    private InputAction surf;

    // Internal velocity and direction variables
    private Vector3 playerVelocity;
    private float surfDirection = 1;

    // Internal rotation variables
    private float rotation = 0.0f;
    private bool justTurned = false;

    void Awake()
    {
        playerInput = new PlayerInput();
        playerVelocity = startingVelocity;
    }

    void OnEnable()
    {
        surf = playerInput.Player.Surf;
        surf.Enable();
        surf.performed += context => doSurf();
        surf.canceled += context => doNeutral();
    }

    void OnDisable()
    {
        playerInput.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        updateVelocity();
        updateTurning();
    }

    void updateVelocity()
    {
        Vector3 desiredVelocity = Vector3.zero;
        float currentAccel = 0.0f;

        if (surfDirection < 0)
        {
            // Go down the wave and speed up
            desiredVelocity.y = maxSpeed.y;
            currentAccel = accel * Time.deltaTime;
        }
        else
        {
            // Go up the wave and slow down
            desiredVelocity.y = 0.0f;
            currentAccel = decel * Time.deltaTime;
        }

        // Incremental velocity update. Stops the surfboard from turning on a dime.
        playerVelocity.y = Mathf.MoveTowards(playerVelocity.y, desiredVelocity.y, currentAccel);

        transform.position += playerVelocity * surfDirection * Time.deltaTime;
    }

    void updateTurning()
    {
        // TODO: Make a transition state when the surfer turns so this doesn't look like magic
        if (justTurned)
        {
            rotation = magicTurnModifier * rotation;
            justTurned = false;
        }

        // Rotate the board
        rotation = surfDirection < 0
            ? Mathf.MoveTowards(rotation, -maxRotation, (playerVelocity.y / maxSpeed.y) * rotationSpeed * Time.deltaTime)
            : Mathf.MoveTowards(rotation, maxRotation, (playerVelocity.y / maxSpeed.y) * rotationSpeed * Time.deltaTime);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, rotation);
    }

    void doSurf()
    {
        surfDirection = Vector2.down.y;
        justTurned = true;
    }

    void doNeutral()
    {
        surfDirection = Vector2.up.y;
        justTurned = true;    
    }

    // added so ScoreManager can get the player speed
    public float GetSpeed()
    {
        return Mathf.Abs(playerVelocity.y);
    }

}
