using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TMP_Text scoreText;
    public Player player;

    public float score = 0f;
    public float scoreMultiplier = 1f;

    private bool gameRunning = false;

    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text endScoreText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        Player.Instance.startGame.AddListener(OnGameStart);
        Player.Instance.endGame.AddListener(OnGameEnd);
    }
    private void OnGameStart()
    {
        gameRunning = true;
    }

    private void OnGameEnd()
    {
        gameRunning = false;

        int finalScore = Mathf.FloorToInt(score);

        if (finalScore > GameManager.Instance.highScore)
        {
            GameManager.Instance.highScore = finalScore;
        }

        endScoreText.text = "Your Score: " + finalScore;
        highScoreText.text = "High Score: " + GameManager.Instance.highScore;
    }

    private void Update()
    {
        if (!gameRunning || player == null)
        {
            return;
        }
        float speed = player.GetSpeed();
        score += speed * scoreMultiplier * Time.deltaTime;

        scoreText.text = Mathf.FloorToInt(score).ToString();
    }

    public void ResetScore()
    {
        score = 0f;
        scoreText.text = "0";
    }
}
