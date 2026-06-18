using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class TutorialText : MonoBehaviour
{
    //step 1 of the tutorial
    [Header("Surf Down")]
    public UnityEvent touchscreenBottomTarget = new UnityEvent();
    public UnityEvent keyboardBottomTarget = new UnityEvent();
    public UnityEvent PSBottomTarget = new UnityEvent();
    public UnityEvent XBoxBottomTarget = new UnityEvent();
    public UnityEvent SwitchBottomTarget = new UnityEvent();

    //step 2 of the tutorial
    [Header("Surf Up")]
    public UnityEvent touchscreenTopTarget = new UnityEvent();
    public UnityEvent keyboardTopTarget = new UnityEvent();
    public UnityEvent PSTopTarget = new UnityEvent();
    public UnityEvent XBoxTopTarget = new UnityEvent();
    public UnityEvent SwitchTopTarget = new UnityEvent();
    
    //step 3 of the tutorial
    [Header("Surf Down Again")]
    public UnityEvent finalTarget = new UnityEvent();

    //end message before going to gameplay
    [Header("End Tutorial Message")]
    public UnityEvent endText = new UnityEvent();

    //type of input (as classified by InputManager's enum named InputType)
    private InputTypeManager.InputType input;


    //TutorialManager.cs calls these functions to change the tutorial text
    //step 1 of tutorial
    public void setBottomTargetText(){
        input = InputTypeManager.Instance.inputType;
        
        if (input == InputTypeManager.InputType.Touchscreen){
            touchscreenBottomTarget.Invoke();
        }
        else if (input == InputTypeManager.InputType.KeyboardMouse){
            keyboardBottomTarget.Invoke();
        }
        else if (input == InputTypeManager.InputType.PSController){
            PSBottomTarget.Invoke();
        }
        else if (input == InputTypeManager.InputType.XBoxController){
            XBoxBottomTarget.Invoke();
        }
        else if (input == InputTypeManager.InputType.SwitchController){
            SwitchBottomTarget.Invoke();
        }
        //if it's none of these options, it stays on the default text
        //  (whatever's in the text box in the Inspector)
    }


    //step 2 of tutorial
    public void setTopTargetText(){
        input = InputTypeManager.Instance.inputType;

        if (input == InputTypeManager.InputType.Touchscreen){
            touchscreenTopTarget.Invoke();
        }
        else if (input == InputTypeManager.InputType.KeyboardMouse){
            keyboardTopTarget.Invoke();
        }
        else if (input == InputTypeManager.InputType.PSController){
            PSTopTarget.Invoke();
        }
        else if (input == InputTypeManager.InputType.XBoxController){
            XBoxTopTarget.Invoke();
        }
        else if (input == InputTypeManager.InputType.SwitchController){
            SwitchTopTarget.Invoke();
        }
        //if it's none of these options, it keeps the text chosen in the
        //  stage before this
    }


    //step 3 of tutorial
    public void setFinalTargetText(){
        finalTarget.Invoke();
    }


    //end message before going to gameplay
    public void setEndText(){
        endText.Invoke();
    }

}
