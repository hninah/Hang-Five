using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public enum PlayerState
    {
        SURFING,
        FLIPPING,
        CRASHING,
        OVER,
        STARTING
    };



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
    [SerializeField] private float trickRotationMin = 45.0f;
    [SerializeField] private float landRotationMax = -45.0f;

    // Internal rotation variables
    private float rotation = 0.0f;

    // Singleton for easier interaction with other scripts (downside: no multiplayer, but we're not doing that?)
    private static Player _instance;
    public static Player Instance { get { return _instance; } }

    // State Control
    [Space(20)]
    [Header("State Control Variables")]
    [SerializeField] private float flipCoolDown = 0.2f;
    private PlayerState state;
    public PlayerState State { get { return state; } }
    [SerializeField] private float waveTopY = 3.0f;
    [SerializeField] private Transform waveBottom;

    [Space(20)]
    [Header("Misc.")]
    [SerializeField] private Animator animator;
    public UnityEvent startGame = new UnityEvent();
    public UnityEvent endGame = new UnityEvent();
    public UnityEvent nextStage = new UnityEvent();

    [Header("Audio")]
    [SerializeField] private AudioClip wipeout;
    private AudioSource audioSource;

    bool crashRoutineRunning = false;
    void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("Player with name: " + gameObject.name + " is being set as the player singleton when " + _instance.gameObject.name + " was previously assigned.");
        }
        _instance = this;

        state = PlayerState.STARTING;
        playerInput = new PlayerInput();
        playerVelocity = startingVelocity;
        rotation = transform.eulerAngles.z;
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        playerInput.Enable();
        surf = playerInput.Player.Surf;
        surf.Enable();
        surf.performed += OnSurfPerformed;
        surf.canceled += OnSurfCanceled;
    }

    void OnDisable()
    {
        surf.performed -= OnSurfPerformed;
        surf.canceled -= OnSurfCanceled;
        playerInput.Disable();
    }

    private void OnSurfPerformed(InputAction.CallbackContext context)
    {
        doSurf();
    }

    private void OnSurfCanceled(InputAction.CallbackContext context)
    {
        doNeutral();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case PlayerState.SURFING:
                updateTurning();
                updateVelocityV2();

                // We want to eventually be able to go back into the flipping state when the timer's done.
                flipImmunityTimer = Mathf.Max(flipImmunityTimer - Time.deltaTime, 0.0f);

                if (transform.position.y >= waveTopY && rotation >= trickRotationMin && GameManager.Instance.tutorialCompleted)
                {
                    playerVelocity.y = playerVelocity.y * Mathf.Abs(rotation / maxUpRotation);
                    state = PlayerState.FLIPPING;
                    animator.SetBool("InAir", true);
                    animator.SetInteger("TrickAnim", Random.Range(0, 2));
                }
                else if ((transform.position.y >= waveTopY && flipImmunityTimer <= 0.0f) || transform.position.y < waveBottom.position.y)
                {
                    print("We crashed going back up into the wave");
                    state = PlayerState.CRASHING;
                }

                break;

            case PlayerState.FLIPPING:
                updateTurning();
                updateFlipVelocity();

                if (transform.position.y >= waveTopY) break;

                // The player should be able to fail at flipping for a risk-reward dynamic
                state = rotation <= landRotationMax 
                    ? PlayerState.SURFING
                    : PlayerState.CRASHING;

                if (state == PlayerState.SURFING)
                {
                    TextParticleManager.Instance.generateScoreParticle(200);
                    ScoreManager.Instance.score += 200;
                    animator.SetBool("InAir", false);
                }

                // Reset this for later (if we didn't our velocity when going up for a flip is inverted)
                flipDirection = 1;
                // Currently need this so we don't immediately go back into FLIPPING
                flipImmunityTimer = flipCoolDown;

                if (state == PlayerState.CRASHING) print("WE CRASHED GOING DOWN WITH ANGLE: " + rotation);

                break;

            case PlayerState.CRASHING:

                if (!crashRoutineRunning)
                {
                    crashRoutineRunning = true;
                    StartCoroutine(HandleCrash());
                }
                break;

            case PlayerState.OVER:
                // We'll figure out what to do with this stuff later
                break;

            case PlayerState.STARTING:
                if (surfDirection < 0)
                {
                    state = PlayerState.SURFING;
                    startGame.Invoke();
                }
                break;
        }

    }

    void OnTriggerEnter2D()
    {
        if (state == PlayerState.CRASHING) return;

        state = PlayerState.CRASHING;
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

    void updateAirVelocity()
    {
        float angleSpeedPercentage = rotation / 90.0f;

        playerVelocity.y = Mathf.MoveTowards(playerVelocity.y, 0.0f, decel * Time.deltaTime);

        transform.position += playerVelocity * angleSpeedPercentage * Time.deltaTime;
    }

    // IDEA?: experiment with making turning speed dependent on velocity?
    void updateTurning()
    {
        rotation = surfDirection < 0
            ? Mathf.MoveTowards(rotation, maxDownRotation, rotationSpeed * Time.deltaTime)
            : Mathf.MoveTowards(rotation, maxUpRotation, deRotationSpeed * Time.deltaTime);

        animator.SetFloat("Rotation", rotation);

        // FIXME: This may lead to floating point error with the x and y rotation (sometimes accumulates error by 0.0001)
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, rotation);
    }

    void doSurf()
    {
        surfDirection = Vector2.down.y;
        animator.SetBool("SurfingDown", true);
    }

    void doNeutral()
    {
        surfDirection = Vector2.up.y;
        animator.SetBool("SurfingDown", false);
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
    // added so ScoreManager can get the player speed
    public float GetSpeed()
    {
        return Mathf.Abs(playerVelocity.y);
    }

    IEnumerator HandleCrash()
    {
        audioSource.PlayOneShot(wipeout, 0.3f);
        animator.SetTrigger("Crashing");
        // wait for 1 second because that is how long the animation is
        yield return new WaitForSeconds(1f);
        Time.timeScale = 1f;
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            print("player crashed in the tutorial");
            TutorialManager.Instance.PlayerCrashed();
            yield break;
        }
        // freeze the game so player cannot gain any more points
        Time.timeScale = 0f;
        int finalScore = Mathf.FloorToInt(ScoreManager.Instance.score);

        //display wipeout screen
        endGame.Invoke();
        //display "Next" button if we passed score threshold
        if (GameManager.Instance.passedCurrentStage(finalScore)){
            nextStage.Invoke();
        }
        
        // game manager gets the final score to compare if stage was passed and high score
        GameManager.Instance.GameOver(finalScore);
    }

    // resets the player for the tutorial 
    public void ResetPlayer()
    {
        crashRoutineRunning = false;
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        state = PlayerState.STARTING;
        playerVelocity = startingVelocity;
        rotation = 0f;

        surfDirection = 1f;
        flipDirection = 1f;
        flipImmunityTimer = 0f;

        transform.eulerAngles = Vector3.zero;

        animator.ResetTrigger("Crashing");
        animator.SetBool("SurfingDown", false);

        // Force animator back to a clean state
        animator.Play("SurferBegin", 0, 0f);
    }

}
