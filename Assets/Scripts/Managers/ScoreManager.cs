using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
        GameManager.Instance.stageCleared = false;
    }

    private void OnGameEnd()
    {
        
        gameRunning = false;

        int finalScore = Mathf.FloorToInt(score);

        endScoreText.text = "Your Score: " + finalScore;

        // infinite mode
        if (GameManager.Instance.currentStage > GameManager.Instance.scoreRequired.Count)
        {
            highScoreText.text = "High Score: " + GameManager.Instance.highScore;
        }
        // player passed the current level
        else if (GameManager.Instance.stageCleared)
        {
            highScoreText.text = "Next Target Score: " + GameManager.Instance.targetScore;
        }
        // player did not pass the level
        else
        {
            highScoreText.text = "Target Score: " + GameManager.Instance.targetScore;
        }
    }

    private void Update()
    {
        if (!gameRunning || player == null || (player.State != Player.PlayerState.SURFING && player.State != Player.PlayerState.FLIPPING))
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
