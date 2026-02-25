using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempButton : MonoBehaviour
{
    public void buttonPressContinue()
    {
        if (!CutsceneManager.Instance.finishedCutscenes)
            SceneManager.LoadScene("Cutscene");
        else
            SceneManager.LoadScene("Gameplay");
    }

    public void buttonPressQuit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
