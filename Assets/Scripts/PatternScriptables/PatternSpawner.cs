using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSpawner : MonoBehaviour
{
    float patternTimer = 0.0f;
    GameObject currentObject;

    // Update is called once per frame
    void Update()
    {
        patternTimer = Mathf.Max(patternTimer - Time.deltaTime, 0.0f);

        if (patternTimer <= 0.0f && currentObject != null)
        {
            print("CREATING OBJECT");
            Instantiate(currentObject, transform.position, Quaternion.identity);
            currentObject = null;
            return;
        }
    }

    public void setPattern(GameObject obj, float timeToSpawn)
    {
        patternTimer = timeToSpawn;
        currentObject = obj;
    }

    public bool hasObject()
    {
        return currentObject != null;
    }
}
