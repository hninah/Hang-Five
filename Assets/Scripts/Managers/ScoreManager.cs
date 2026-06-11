using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TMP_Text scoreText;
    public Player player;

    public float score = 0f;
    public float scoreMultiplier = 1f;

    private bool gameRunning = false;
    private bool didScoreFX = false;
    public UnityEvent targetScoreFX = new UnityEvent();

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

        //set score text to white if it's night
        if (GameManager.Instance.getBackground() == "Night"){
            scoreText.color = Color.white;
        }
    }

    private void OnGameEnd()
    {
        Debug.Log("OnGameEnd beatBoss = " + GameManager.Instance.beatBoss);
        gameRunning = false;
        didScoreFX = false; //reset this for next level

        int finalScore = Mathf.FloorToInt(score);
        endScoreText.text = "Your Score: " + finalScore;
        // beat the boss so from now on show High score instead of target score
        if (GameManager.Instance.beatBoss && GameManager.Instance.inBossLevel)
        {
            // beat the boss and theres no high score or target score for this one death screen
            highScoreText.gameObject.SetActive(false);
            return;
        }
        else if (GameManager.Instance.beatBoss)
        {
            // beat the boss no longer in the boss level so player is in the infinite mode where high score is displayed
            highScoreText.gameObject.SetActive(true);
            highScoreText.text = "High Score: " + GameManager.Instance.highScore;
            return;
        }
        // player passed the current level
        else if (GameManager.Instance.stageCleared)
        {
            highScoreText.text = "Next Target Score: " + GameManager.Instance.targetScore;
        }
        // player did not pass the level or the boss level
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

        //effects when player passes current target
        if (!GameManager.Instance.beatBoss && 
                score >= GameManager.Instance.targetScore && !didScoreFX)
        {
            didScoreFX = true;
            targetScoreFX.Invoke();
        }
    }

    public void ResetScore()
    {
        score = 0f;
        scoreText.text = "0";
    }
}
