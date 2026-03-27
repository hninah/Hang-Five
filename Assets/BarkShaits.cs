using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkShaits : MonoBehaviour
{
    public GameObject barkShaits;
    public float delay = 3f;

    public LaneSpawner laneSpawner;

    private bool isActive = false;
    private Vector3 basePosition;

    public float amplitude = 1.5f;
    public float frequency = 2f;
    void Start()
    {
        print("in the boss level");
        ScoreManager.Instance.ResetScore();
        laneSpawner.DisableSpawning();
        // player is gonna just be surfing with no obstacles and then after a bit the boss will come in
        StartCoroutine(SpawnBoss());
    }
    void Update()
    {
        // makes the boss go up and down
        if (!isActive) return;

        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;

        barkShaits.transform.position = basePosition + new Vector3(0, yOffset, 0);
    }
    IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(delay);

        barkShaits.SetActive(true);
        // start spawning obstacles
        laneSpawner.EnableSpawning();

        // move bark from the right side of the screen to where he should be 
        Vector3 startPos = new Vector3(15f, barkShaits.transform.position.y, 0);
        Vector3 endPos = new Vector3(5f, barkShaits.transform.position.y, 0);

        barkShaits.transform.position = startPos;

        float t = 0;
        // moving bark
        while (t < 1)
        {
            t += Time.deltaTime;
            barkShaits.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        basePosition = endPos;
        isActive = true;
    }
}
