using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator menuAnimator;
    public void OnStartPressed()
    {
        menuAnimator.SetTrigger("StartGame");
    }
}
