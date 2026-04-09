using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPatternManager : MonoBehaviour
{
    public List<PatternSpawner> spawners;
    public List<PatternScriptable> patterns;
    private int currentPattern;
    private float coolDownTimer = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Comment this out if you want to add your own patterns for testing purposes
        patterns = GameManager.Instance.GetActivePatterns();

        currentPattern = Random.Range(0, patterns.Count);

        // We want to load the patterns, then shut this off so it doesn't update until time is right.
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        coolDownTimer = Mathf.Max(coolDownTimer - Time.deltaTime, 0.0f);
        if (coolDownTimer > 0.0f || !patternDone())
        {
            return;
        }

        spawnPattern();
    }

    void spawnPattern()
    {
        for (int i = 0; i < patterns[currentPattern].obstacles.Length; ++i)
        {
            int spawnerIdx = patterns[currentPattern].spawnPointIdxs[i];
            GameObject obstacle = patterns[currentPattern].obstacles[i];
            float timeTillSpawn = patterns[currentPattern].timeTillSpawn[i];

            spawners[spawnerIdx].setPattern(obstacle, timeTillSpawn, patterns[currentPattern].isRandomSpawning[i]);
        }

        coolDownTimer = patterns[currentPattern].coolDownTime;
        currentPattern = Random.Range(0, patterns.Count);
    }

    bool patternDone()
    {
        foreach (PatternSpawner spawner in spawners)
        {
            if (!spawner.hasObject()) {
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
