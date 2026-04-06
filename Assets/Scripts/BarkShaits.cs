using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkShaits : MonoBehaviour
{
    public float delay = 3f;

    public LaneSpawner laneSpawner;

    private bool isActive = false;
    private Vector3 basePosition;

    public float amplitude = 1.5f;
    public float frequency = 2f;
    void Start()
    {
        if (!GameManager.Instance.inBossLevel)
        {
            gameObject.SetActive(false);
            return;
        }
        print("in the boss level");
        // player is gonna just be surfing with no obstacles and then after a bit the boss will come in
        Player.Instance.startGame.AddListener(OnGameStart);
    }
    void Update()
    {
        // makes the boss go up and down
        if (!isActive) return;

        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;

        gameObject.transform.position = basePosition + new Vector3(0, yOffset, 0);
    }
    void OnGameStart()
    {
        ScoreManager.Instance.ResetScore();
        laneSpawner.DisableSpawning();
        StartCoroutine(SpawnBoss());
    }
    IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(delay);

        // start spawning obstacles
        laneSpawner.EnableSpawning();

        // move bark from the right side of the screen to where he should be 
        Vector3 startPos = new Vector3(15f, gameObject.transform.position.y, 0);
        Vector3 endPos = new Vector3(5f, gameObject.transform.position.y, 0);

        gameObject.transform.position = startPos;

        float t = 0;
        // moving bark
        while (t < 1)
        {
            t += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        basePosition = endPos;
        isActive = true;
    }
}
