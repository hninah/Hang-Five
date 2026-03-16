using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuAnimEvents : MonoBehaviour
{
    public void OnStandingUpFinished()
    {
        if (!GameManager.Instance.tutorialCompleted)
        {
            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            SceneManager.LoadScene("Gameplay");
        }
    }
}
