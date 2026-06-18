using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TempButton : MonoBehaviour
{

    public GameObject retryButton;
    public GameObject nextButton;
    private Vector3 startPos;
    public float hoverScale = 1.15f;
    private bool hasPressedContinue = false;

    //display a warning the first time the player tries to quit
    private bool hasPressedQuit = false;
    public UnityEvent quitMessage = new UnityEvent();

    void Start()
    {
        startPos = transform.localPosition;

        bool passed = GameManager.Instance.stageCleared;

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
        if (hasPressedContinue) return;
        hasPressedContinue = true;
        Time.timeScale = 1f;
        if (!CutsceneManager.Instance.isFinished() && GameManager.Instance.stageCleared)
        {
            SceneTransitioner.Instance.LoadScene("Cutscene");
        }
        else
        {
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
        //display message
        if (!hasPressedQuit){
            hasPressedQuit = true; //record the first time we press Quit
            quitMessage.Invoke();
            return;
        } 

        Time.timeScale = 1f;
        CutsceneManager.Instance.resetCutscenes();
        GameManager.Instance.resetGameState();
        SceneTransitioner.Instance.LoadScene("MainMenu");
    }


}
