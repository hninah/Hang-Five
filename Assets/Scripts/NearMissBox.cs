using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearMissBox : MonoBehaviour
{
    // Implementation
    [SerializeField] private int nearMissBonus = 100;
    [SerializeField] private float nearMissCoolDown = 0.1f;
    private float nearMissCoolDownTimer = 0.0f;

    // Visuals
    [SerializeField] private SpriteRenderer sprite;
    int numNearMisses = 0;

    void Start()
    {
        nearMissCoolDownTimer = nearMissCoolDown;
    }

    void OnTriggerEnter2D()
    {
        if (Player.Instance.State == Player.PlayerState.CRASHING) return;

        numNearMisses++;

        sprite.enabled = true;
    }

    void OnTriggerStay2D()
    {
        if (Player.Instance.State == Player.PlayerState.CRASHING || nearMissCoolDownTimer > 0.0f) return;

        ScoreManager.Instance.score += nearMissBonus;
        TextParticleManager.Instance.generateScoreParticle(nearMissBonus);
        nearMissCoolDownTimer = nearMissCoolDown;
    }

    void OnTriggerExit2D()
    {
        numNearMisses = Mathf.Max(0, numNearMisses - 1);

        if (numNearMisses == 0)
        {
            sprite.enabled = false;
        }
    }

    void Update()
    {
        nearMissCoolDownTimer = nearMissCoolDownTimer < 0.0f ? 0 : nearMissCoolDownTimer - Time.deltaTime;
    }
}
