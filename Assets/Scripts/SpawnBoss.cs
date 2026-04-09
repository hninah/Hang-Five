using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    public GameObject boss;
    private bool hasSpawned = false;

    private void Start()
    {
        boss.SetActive(false);
    }

    void Update()
    {
        if (boss == null) return;

        // spawn boss when entering boss stage
        if (!hasSpawned && GameManager.Instance.inBossLevel)
        {
            boss.SetActive(true);
            hasSpawned = true;
        }

        // hide boss after boss defeated
        if (hasSpawned && !GameManager.Instance.inBossLevel)
        {
            boss.SetActive(false);
        }

    }
}