using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTarget : MonoBehaviour
{
    Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        float pulse = 1f + Mathf.Sin(Time.time * 4f) * 0.15f;
        transform.localScale = startScale * pulse;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        TutorialManager.Instance.PlayerHitTarget(gameObject);
        gameObject.SetActive(false);
    }
}