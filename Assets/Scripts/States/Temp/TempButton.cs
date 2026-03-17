using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempButton : MonoBehaviour
{
    public void buttonPressContinue()
    {
        Time.timeScale = 1f;
        if (!CutsceneManager.Instance.isFinished() && GameManager.Instance.stageCleared)
        {
            GameManager.Instance.stageCleared = false;
            SceneManager.LoadScene("Cutscene");
        }
        else
        {
            GameManager.Instance.stageCleared = false;
            SceneManager.LoadScene("Gameplay");
        }
    }

    public void buttonPressQuit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
