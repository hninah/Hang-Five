using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class ContinueText : MonoBehaviour
{
    //instructions for how to proceed after wipeout
    [Header("Continue Instructions")]
    public UnityEvent touchscreenNext = new UnityEvent();
    public UnityEvent keyboardNext = new UnityEvent();
    
    //type of input (as classified by InputManager's enum named InputType)
    private InputManager.InputType input;


    void Start(){
        //change the instruction text based on the input type
        input = InputManager.Instance.inputType;

        if (input == InputManager.InputType.Touchscreen){
            touchscreenNext.Invoke();
        }
        else if (input == InputManager.InputType.KeyboardMouse){
            keyboardNext.Invoke();
        }
        //if it's none of these options, it stays on the default text
        //  (whatever's in the text box in the Inspector)
    }

}
