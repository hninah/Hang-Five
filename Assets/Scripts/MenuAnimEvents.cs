using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuAnimEvents : MonoBehaviour
{
    public void OnStandingUpFinished()
    {
        SceneManager.LoadScene("version1");
    }
}
