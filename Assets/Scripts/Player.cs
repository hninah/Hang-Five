using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public enum PlayerState
    {
        SURFING,
        FLIPPING,
        CRASHING,
        OVER
    };

    /* TODO(?):
     * - Add a turn transition with weight to it based on how fast the player is moving
     *          - NOTE: The board now slows when rotating, but it's not necessarily dependent on velocity
     * - Add collision with obstacles
    */
    [Header("Surfing Movement Variables")]
    [SerializeField] private float accel;
    [SerializeField] private float decel;
    [SerializeField] private Vector3 maxSpeed;
    [SerializeField] private Vector3 startingVelocity;

    [Space(20)]
    [Header("Surfing Turning Variables (Degrees)")]
    [Tooltip("Maximum Rotation The Board Will Realistically Go Upwards.")]
    [SerializeField] private float maxUpRotation;
    [Tooltip("Maximum Rotation The Board Will Realistically Go Downwards.")]
    [SerializeField] private float maxDownRotation;
    [Tooltip("Minimum Rotation For The Board To Go 'Downwards' (accelerate).")]
    [SerializeField] private float downRotationMin;
    [Tooltip("Maximum Rotation For The Board To Go 'Downwards' (accelerate).")]
    [SerializeField] private float downRotationMax;
    [Tooltip("Minimum Rotation For The Board To Go 'Upwards' (decelerate).")]
    [SerializeField] private float upRotationMin;
    [Tooltip("Maximum Rotation For The Board To Go 'Upwards' (decelerate).")]
    [SerializeField] private float upRotationMax;
    [Tooltip("Speed of Rotation On Button Press.")]
    [SerializeField] private float rotationSpeed;
    [Tooltip("Speed of Rotation WHen Button Is Not Pressed.")]
    [SerializeField] private float deRotationSpeed;

    [Space(20)]
    [Header("Flipping Movement Variables")]
    [SerializeField] private float gravity;
    [SerializeField] private float fastFallMultiplier;

    [Space(20)]
    [Header("Flipping Turning Variables")]
    [SerializeField] private float flipRotationSpeed;

    // Input handlers
    private PlayerInput playerInput;
    private InputAction surf;

    // Internal velocity and direction variables
    private Vector3 playerVelocity;
    private float surfDirection = 1;
    private float flipDirection = 1;
    private float flipImmunityTimer = 0.0f;

    // Internal rotation variables
    private float rotation = 0.0f;

    // State Control
    [Space(20)]
    [Header("State Control Variables")]
    [SerializeField] private float flipCoolDown = 0.2f;
    public PlayerState state;

    void Awake()
    {
        state = PlayerState.SURFING;
        playerInput = new PlayerInput();
        playerVelocity = startingVelocity;
        rotation = transform.eulerAngles.z;
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
        switch(state)
        {
            case PlayerState.SURFING:
                updateTurning();
                updateVelocityV2();

                // We want to eventually be able to go back into the flipping state when the timer's done.
                flipImmunityTimer = Mathf.Max(flipImmunityTimer - Time.deltaTime, 0.0f);

                // FIXME: replace the transform.position condition with a non-placeholder
                // Add the timer so we don't infinitely get stuck in a flipping state
                // The surfDirection is for preference. It does cause a bug where you can surf
                // above the top of the wave, but I'm not sure what I want to do with this yet.
                if (transform.position.y >= 3.0f && flipImmunityTimer <= 0.0f && surfDirection > 0)
                {
                    state = PlayerState.FLIPPING;
                }

                break;

            case PlayerState.FLIPPING:
                updateFlipRotation();
                updateFlipVelocity();

                // FIXME: Replace with a non-placeholder condition
                if (transform.position.y >= 3.0f) break;

                // The player should be able to fail at flipping for a risk-reward dynamic
                state = rotation >= downRotationMax && rotation <= upRotationMax
                    ? PlayerState.SURFING
                    : PlayerState.CRASHING;

                // Reset this for later (if we didn't our velocity when going up for a flip is inverted)
                flipDirection = 1;
                // Currently need this so we don't immediately go back into FLIPPING
                flipImmunityTimer = flipCoolDown;

                break;

            case PlayerState.CRASHING:
                print("YOU CRASHED");
                break;

            case PlayerState.OVER:
                print("NOT IMPORTANT YET");
                break;
        }

    }

    // OLD VERSION: REMAINS IN CODE FOR COMPARISON
    void updateVelocityV1()
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

    void updateVelocityV2()
    {
        // Don't go full speed in the y direction when turning (effectively a sine function for our purposes)
        float angleSpeedPercentage = rotation / 90.0f;

        if (rotation <= downRotationMin && rotation >= downRotationMax)
        {
            // Accelerate when surfing down the wave
            playerVelocity.y = Mathf.MoveTowards(playerVelocity.y, maxSpeed.y, accel * Time.deltaTime);
        }
        else if (rotation >= upRotationMin && rotation <= upRotationMax)
        {
            // Decelerate when surfing up the wave
            playerVelocity.y = Mathf.MoveTowards(playerVelocity.y, 0.0f, decel * Time.deltaTime);
        }

        transform.position += playerVelocity * angleSpeedPercentage * Time.deltaTime;
    }

    // IDEA?: experiment with making turning speed dependent on velocity?
    void updateTurning()
    {
        rotation = surfDirection < 0
            ? Mathf.MoveTowards(rotation, maxDownRotation, rotationSpeed * Time.deltaTime)
            : Mathf.MoveTowards(rotation, maxUpRotation, deRotationSpeed * Time.deltaTime);

        // FIXME: This may lead to floating point error with the x and y rotation (sometimes accumulates error by 0.0001)
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, rotation);
    }

    void doSurf()
    {
        surfDirection = Vector2.down.y;
    }

    void doNeutral()
    {
        surfDirection = Vector2.up.y;  
    }

    void updateFlipRotation()
    {
        if (surfDirection < 0 && flipDirection < 0) return;

        // Make sure the rotation is always in [-180, 180] so we're using correct rotations when we get back to surfing
        rotation = Mathf.MoveTowards(rotation, 181, flipRotationSpeed * Time.deltaTime);
        if (rotation >= 180) rotation -= 360;

        // FIXME: This may lead to floating point error with the x and y rotation (sometimes accumulates error by 0.0001)
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, rotation);
    }

    void updateFlipVelocity()
    {
        // Make gravity more prevalent when the player wants to go back down to the wave
        float multiplier = surfDirection < 0 ? fastFallMultiplier : 1.0f;

        // Going up to the top of our arc or back down to the wave
        playerVelocity.y = flipDirection > 0
            ? Mathf.MoveTowards(playerVelocity.y, 0.0f, gravity * Time.deltaTime)
            : Mathf.MoveTowards(playerVelocity.y, maxSpeed.y, gravity * fastFallMultiplier * Time.deltaTime);

        // When we've reached the top of our arc, we'll flip the velocity so we go back down.
        // We use 0.0001f because MoveTowards isn't guaranteed to ever reach it's target.
        if (playerVelocity.y <= 0.0001f)
        {
            flipDirection = -1;
        }

        transform.position += playerVelocity * flipDirection * Time.deltaTime;
    }
}
