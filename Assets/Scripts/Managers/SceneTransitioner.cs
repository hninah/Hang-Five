using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    private static SceneTransitioner _instance;
    public static SceneTransitioner Instance { get { return _instance; } }

    [SerializeField] public Animator transitionAnimator;
    [SerializeField] private float transitionTime = 1.0f;

    void Awake()
    {
        _instance = this;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(SceneTransition(sceneName));
    }

    private IEnumerator SceneTransition(string sceneName)
    {
        transitionAnimator.SetTrigger("ToOut");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }
}
