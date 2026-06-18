using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuAnimEvents : MonoBehaviour
{
    private Vector3 startPos;
    public float hoverScale = 1.15f;
    private void Start()
    {
        startPos = transform.localPosition;
    }
    void Update()
    {
        // player should go up and down a little
        float y = Mathf.Sin(Time.time * 1.5f) * 0.1f;
        transform.localPosition = startPos + new Vector3(0, y, 0);
    }
    public void OnStandingUpFinished()
    {
        if (!GameManager.Instance.tutorialCompleted)
        {
            //SceneManager.LoadScene("AnimatedCutscene");
            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            SceneManager.LoadScene("Gameplay");
        }
    }
}
