using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class BeginGameText : MonoBehaviour
{
    float timer = 5.0f;
    public TMP_Text text;

    //set text based on input type
    [Header("Start Game Prompt")]
    public UnityEvent touchscreenPrompt = new UnityEvent();
    public UnityEvent keyboardPrompt = new UnityEvent();
    public UnityEvent PSPrompt = new UnityEvent();
    public UnityEvent XBoxPrompt = new UnityEvent();
    public UnityEvent SwitchPrompt = new UnityEvent();

    //type of input (as classified by InputManager's enum named InputType)
    private InputTypeManager.InputType input;


    // Start is called before the first frame update
    void Start(){
        input = InputTypeManager.Instance.inputType;

        if (input == InputTypeManager.InputType.Touchscreen){
            touchscreenPrompt.Invoke();
        }
        else if (input == InputTypeManager.InputType.KeyboardMouse){
            keyboardPrompt.Invoke();
        }
        else if (input == InputTypeManager.InputType.PSController){
            PSPrompt.Invoke();
        }
        else if (input == InputTypeManager.InputType.XBoxController){
            XBoxPrompt.Invoke();
        }
        else if (input == InputTypeManager.InputType.SwitchController){
            SwitchPrompt.Invoke();
        }
        //if it's none of these options, it stays on the default text
        //  (whatever's in the text box in the Inspector)
    }


    // Update is called once per frame
    void Update()
    {
        if (text.enabled)
        {
            return;
        }

        if (Player.Instance.State == Player.PlayerState.SURFING)
        {
            Destroy(gameObject);
        }

        timer -= Time.deltaTime;

        if (timer <= 0.0f)
        {
            text.enabled = true;
        }
    }
}
