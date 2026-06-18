using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{

    public void OnClick(){
        Application.Quit();
        Debug.Log("quitting application");
    }
}
