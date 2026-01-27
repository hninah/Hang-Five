using UnityEngine;

public class LaneSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public Transform[] laneAnchors;    
    public float spawnX = 12f;
    public float minDelay = 0.6f;
    public float maxDelay = 1.3f;

    float t;

    void Start()
    {
        t = Random.Range(minDelay, maxDelay);
    }

    void Update()
    {
        t -= Time.deltaTime;
        if (t <= 0f)
        {
            int lane = Random.Range(0, laneAnchors.Length);
            Vector3 pos = new Vector3(spawnX, laneAnchors[lane].position.y, 0f);
            Instantiate(obstaclePrefab, pos, Quaternion.identity);
            t = Random.Range(minDelay, maxDelay);
        }
    }
}
