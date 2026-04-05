using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TempButton : MonoBehaviour
{

    public GameObject retryButton;
    public GameObject nextButton;
    private Vector3 startPos;
    public float hoverScale = 1.15f;
    private bool hasPressed = false;

    void Start()
    {
        startPos = transform.localPosition;

        bool passed = GameManager.Instance.stageCleared && !GameManager.Instance.inInfiniteMode;

        retryButton.SetActive(!passed); // show Retry if failed
        nextButton.SetActive(passed);   // show Next if passed
    }

    void Update()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected == gameObject)
        {
            // float if selected
            float t = Time.time;
            float y = Mathf.Sin(t * 4f) * 5f; // slightly stronger effect
            transform.localPosition = startPos + new Vector3(0, y, 0);
        }
        else
        {
            // reset position if not selected
            transform.localPosition = startPos;
        }
    }
    public void buttonPressContinue()
    {
        if (hasPressed) return;
        hasPressed = true;
        Time.timeScale = 1f;
        if (!CutsceneManager.Instance.isFinished() && GameManager.Instance.stageCleared)
        {
            SceneManager.LoadScene("Cutscene");
        }
        else
        {
            // if we just finished the boss, enter infinite mode
            if (!GameManager.Instance.inInfiniteMode &&
                GameManager.Instance.currentStage == GameManager.Instance.scoreRequired.Count)
            {
                GameManager.Instance.inInfiniteMode = true;
                GameManager.Instance.currentStage = GameManager.Instance.scoreRequired.Count + 1;
            }

            SceneManager.LoadScene("Gameplay");
            GameManager.Instance.stageCleared = false;
        }
    }

    public void buttonPressRetry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Gameplay");
    }

    public void buttonPressQuit()
    {
        Time.timeScale = 1f;
        CutsceneManager.Instance.resetCutscenes();
        GameManager.Instance.resetGameState();
        SceneManager.LoadScene("MainMenu");
    }


}
