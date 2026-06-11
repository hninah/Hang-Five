using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class InstructionsText : MonoBehaviour
{
    //basic instructions during cutscene
    [Header("Instructions for Next Dialogue")]
    public UnityEvent keyboardNext = new UnityEvent();
    public UnityEvent touchscreenNext = new UnityEvent();
    public UnityEvent PSNext = new UnityEvent();
    public UnityEvent XBoxNext = new UnityEvent();
    public UnityEvent SwitchNext = new UnityEvent();

    //final instruction: CutsceneController invokes these 
    // when it detects that we're on the last dialogue line
    [Header("Instruction at Last Dialogue")]
    public UnityEvent keyboardEnd = new UnityEvent();
    public UnityEvent touchscreenEnd = new UnityEvent();
    public UnityEvent PSEnd = new UnityEvent();
    public UnityEvent XBoxEnd = new UnityEvent();
    public UnityEvent SwitchEnd = new UnityEvent();

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


    public void lastLine(){
        if (input == InputTypeManager.InputType.Touchscreen){
            touchscreenEnd.Invoke();
        }
        else if (input == InputTypeManager.InputType.KeyboardMouse){
            keyboardEnd.Invoke();
        }
        else if (input == InputTypeManager.InputType.PSController){
            PSEnd.Invoke();
        }
        else if (input == InputTypeManager.InputType.XBoxController){
            XBoxEnd.Invoke();
        }
        else if (input == InputTypeManager.InputType.SwitchController){
            SwitchEnd.Invoke();
        }
        //if it's none of these options, it keeps the text chosen in Start()
    }
}
