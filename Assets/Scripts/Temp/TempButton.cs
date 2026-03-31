using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TempButton : MonoBehaviour
{

    private Vector3 startPos;
    public float hoverScale = 1.15f;


    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float t = Time.time;
        float y = Mathf.Sin(t * 2f) * 3f;
        transform.localPosition = startPos + new Vector3(0, y, 0);
    }
    public void buttonPressContinue()
    {
        Time.timeScale = 1f;
        // load a cutscene
        if (!CutsceneManager.Instance.isFinished() && GameManager.Instance.stageCleared)
        {
            SceneManager.LoadScene("Cutscene");
        }
        // load regular gameplay level
        else
        {
            SceneManager.LoadScene("Gameplay");
            GameManager.Instance.stageCleared = false;
        }
    }

    public void buttonPressQuit()
    {
        Time.timeScale = 1f;
        CutsceneManager.Instance.resetCutscenes();
        GameManager.Instance.resetGameState();
        SceneManager.LoadScene("MainMenu");
    }

    public void buttonPressRetry(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("Gameplay");
    }

}
