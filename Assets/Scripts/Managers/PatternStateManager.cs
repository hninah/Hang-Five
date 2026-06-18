using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternStateManager : MonoBehaviour
{
    public enum State
    {
        TRANSITION_TIMING,
        COOLDOWN,
        SPAWNING,
        SPAWN_WAIT,
        ENDLESS_TRANSITION
    }

    private static PatternStateManager _instance;
    public static PatternStateManager Instance { get { return _instance; } }
    public List<PatternSpawner> spawners;
    public PatternState currentState;
    private float coolDownTimer = 2.0f;
    private float transitionCheckTimer = 0.0f;
    public State managerState = State.COOLDOWN;
    [SerializeField] private bool debugging = false;
    public bool Debugging { get { return debugging; } }
    [SerializeField] private PatternState debugStartState;
    private float rngNum = 0.0f;
    public float RngNum { get { return rngNum; } }

    private bool inEndless = false;
    [SerializeField] private bool debugEndless = false;

    // Start is called before the first frame update
    void Start()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }

        if (GameManager.Instance != null && GameManager.Instance.beatBoss)
        {
            inEndless = true;
        }

        if (debugEndless)
        {
            debugging = true;
            inEndless = true;
        }

        _instance = this;
        currentState = getStartingState();
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // ORDER:
        // COOLDOWN (Time before the next pattern spawns)
        // SPAWNING (Spawn the next pattern)
        // SPAWN_WAIT (Wait for the pattern to finish spawning)
        // TRANSITION_TIMING (Wait to check for which state to transition to next)
        switch (managerState)
        {
            case State.COOLDOWN:
                coolDownTimer = Mathf.Max(coolDownTimer - Time.deltaTime, 0.0f);

                managerState = coolDownTimer <= 0.0f ? State.SPAWNING : managerState;
                break;

            case State.TRANSITION_TIMING:
                transitionCheckTimer = Mathf.Max(transitionCheckTimer - Time.deltaTime, 0.0f);

                if (transitionCheckTimer > 0.0f)
                {
                    break;
                }

                rngNum = Random.Range(0.0f, 1.0f);

                if (!inEndless)
                {
                    selectPattern();
                }
                else
                {
                    selectEndlessPattern();
                }

                managerState = State.COOLDOWN;
                break;

            case State.SPAWNING:
                spawnPattern();

                managerState = State.SPAWN_WAIT;
                break;

            case State.SPAWN_WAIT:
                if (!patternDone())
                {
                    break;
                }

                transitionCheckTimer = currentState.conditionCheckTimer;
                managerState = State.TRANSITION_TIMING;
                break;
        }
    }

    private void selectPattern()
    {
        int j = 1;
        int numTransitions = currentState.transitions.Length;

        foreach (PatternStateTransition transition in currentState.transitions)
        {
            if (transition.stateTransition(Player.Instance))
            {
                currentState = transition.patternState;
                coolDownTimer = transition.coolDownTime;
                break;
            }

            j++;
        }

        if (j > numTransitions)
        {
            Debug.LogError("Unable to find valid pattern state transition. Re-using current state.");
            coolDownTimer = 2.0f;
        }
    }

    private void selectEndlessPattern()
    {
        if (currentState.transitions.Length == 0)
        {
            print("We have no transitions.");
            return;
        }

        // We've met the transition to the next score checkpoint
        if (currentState.transitions[0].stateTransition(Player.Instance))
        {
            currentState = currentState.transitions[0].getNextState();
            coolDownTimer = currentState.transitions[0].coolDownTime;
            return;
        }

        // I don't want to have to write out a billion probability conditions, so we're doing this instead
        currentState = currentState.transitions[1].getNextState();
        coolDownTimer = currentState.transitions[1].coolDownTime;
    }

    private void spawnPattern()
    {
        for (int i = 0; i < currentState.currentPattern.obstacleInfoArr.Length; ++i)
        {
            ObstaclePatternInfo info = currentState.currentPattern.obstacleInfoArr[i];

            spawners[info.spawnIdx].setPattern(info.obstacle, info.timeTillSpawn, info.randomSpawnY);
        }
    }

    private PatternState getStartingState()
    {
        Debug.Log($"Pattern State Debugging: {debugging}");

        PatternState state = GameManager.Instance != null ? GameManager.Instance.getStartingState() : null;

        if (!debugging && state != null)
        {
            return state;
        }

        return debugStartState;
    }

    bool patternDone()
    {
        foreach (PatternSpawner spawner in spawners)
        {
            if (!spawner.hasObject())
            {
                continue;
            }

            return false;
        }

        return true;
    }

    public void pausePattern()
    {
        foreach (PatternSpawner spawner in spawners)
        {
            spawner.enabled = false;
        }
    }

    public void unpausePattern()
    {
        foreach (PatternSpawner spawner in spawners)
        {
            spawner.enabled = true;
        }
    }
}
