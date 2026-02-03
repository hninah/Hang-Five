using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public Text scoreText;
    public Player player;

    public float score = 0f;
    public float scoreMultiplier = 1f;

    private void Awake()
    {
        //ensures there is only one instance of the ScoreManager at one time
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        // updates score based on player speed
        float speed = player.GetSpeed();
        score += speed * scoreMultiplier * Time.deltaTime;

        scoreText.text = Mathf.FloorToInt(score).ToString();
    }

}
