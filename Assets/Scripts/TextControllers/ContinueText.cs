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
    public UnityEvent PSNext = new UnityEvent();
    public UnityEvent XBoxNext = new UnityEvent();
    public UnityEvent SwitchNext = new UnityEvent();
    
    //type of input (as classified by InputManager's enum named InputType)
    private InputTypeManager.InputType input;


    void Start(){
        //change the instruction text based on the input type
        input = InputTypeManager.Instance.inputType;

        if (input == InputTypeManager.InputType.Touchscreen){
            touchscreenNext.Invoke();
        }
        else if (input == InputTypeManager.InputType.KeyboardMouse){
            keyboardNext.Invoke();
        }
        else if (input == InputTypeManager.InputType.PSController){
            PSNext.Invoke();
        }
        else if (input == InputTypeManager.InputType.XBoxController){
            XBoxNext.Invoke();
        }
        else if (input == InputTypeManager.InputType.SwitchController){
            SwitchNext.Invoke();
        }
        //if it's none of these options, it stays on the default text
        //  (whatever's in the text box in the Inspector)
    }

}
