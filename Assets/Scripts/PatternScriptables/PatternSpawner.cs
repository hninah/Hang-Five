using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSpawner : MonoBehaviour
{
    float patternTimer = 0.0f;
    GameObject currentObject;
    bool randomSpawn = false;

    // Update is called once per frame
    void Update()
    {
        patternTimer = Mathf.Max(patternTimer - Time.deltaTime, 0.0f);

        if (patternTimer <= 0.0f && currentObject != null)
        {
            Vector3 spawnPosition = transform.position;

            if (randomSpawn)
            {
                spawnPosition.y = Random.Range(-4.2f, 1.85f);
                randomSpawn = false;
            }

            GameObject prefab = Instantiate(currentObject, spawnPosition, Quaternion.identity) as GameObject;
            EnemyPauser.Instance.addObstacle(prefab.GetComponent<Obstacle>());

            currentObject = null;
            return;
        }
    }

    public void setPattern(GameObject obj, float timeToSpawn, bool isRandom)
    {
        patternTimer = timeToSpawn;
        currentObject = obj;
        randomSpawn = isRandom;
    }

    public bool hasObject()
    {
        return currentObject != null;
    }
}
