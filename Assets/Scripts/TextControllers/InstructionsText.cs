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

    //final instruction: CutsceneController invokes these 
    // when it detects that we're on the last dialogue line
    [Header("Instruction at Last Dialogue")]
    public UnityEvent keyboardEnd = new UnityEvent();
    public UnityEvent touchscreenEnd = new UnityEvent();

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


    public void lastLine(){
        if (input == InputManager.InputType.Touchscreen){
            touchscreenEnd.Invoke();
        }
        else if (input == InputManager.InputType.KeyboardMouse){
            keyboardEnd.Invoke();
        }
        //if it's none of these options, it keeps the text chosen in Start()
    }
}
