using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class MainMenu : MonoBehaviour
{
    //surfer stand-up animation
    public Animator menuAnimator;
    

    public void OnStartPressed()
    {
        menuAnimator.SetTrigger("StartGame"); 
    }


    public void OnEndlessPressed()
    {
        GameManager.Instance.startEndlessMode();
        menuAnimator.SetTrigger("StartGame"); 
    }
}
