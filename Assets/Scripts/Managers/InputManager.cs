using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    //add new supported input types here as needed
    public enum InputType
    { 
        Touchscreen, 
        KeyboardMouse
    }

    //variables
    public static InputManager Instance;
    public InputType inputType;


    //called when InputManager is created
    private void Awake(){

        //don't destroy on load so that all scenes can access this
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);

            //Use this section to assign the enum value based on 
            //  the connected device type
            //note: this assumes our input type will never change mid-game
            if(Touchscreen.current != null){
                //found a connected touchscreen
                inputType = InputType.Touchscreen;
            }
            else if (Keyboard.current != null){
                //found a connected keyboard
                inputType = InputType.KeyboardMouse;
            }
            //add more device types here as needed
            else{
                Debug.Log("no supported devices found: make sure you have options for all supported devices!");
            }

        }
        else{
            Destroy(gameObject);
        }
    }

}
