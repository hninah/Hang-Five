using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class MainMenu : MonoBehaviour
{
    public Animator menuAnimator;
    
    //use to make the selected button float
    private Vector3 startPos;
    public float hoverScale = 1.15f;
    private bool hasPressed = false; ///


    void Start()
    {
        startPos = transform.localPosition;
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
