using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputTypeManager : MonoBehaviour
{
    //add new supported input types here as needed
    public enum InputType
    { 
        Touchscreen, 
        KeyboardMouse,
        PSController,
        XBoxController,
        SwitchController
    }

    //variables
    public static InputTypeManager Instance;
    public InputType inputType;

    //called when InputManager is created
    private void Awake(){

        //don't destroy on load so that all scenes can access this
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

}
