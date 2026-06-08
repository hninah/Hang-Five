using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator animator;
    private Vector3 startPos;
    public float hoverScale = 1.15f;

    //input actions to generalize for different inputs
    private PlayerInput playerInput;
    private InputAction submit;


    void Awake(){
        playerInput = new PlayerInput();
        animator = GetComponent<Animator>();
        startPos = transform.localPosition;
    }

    void OnEnable(){
        playerInput.Enable();
        submit = playerInput.UI.Submit;
        submit.Enable();
        submit.performed += OnSubmitPerformed;
    }

    void OnDisable(){
        submit.performed -= OnSubmitPerformed;
        playerInput.Disable();
    }

    //run button animation on the selected button
    void OnSubmitPerformed(InputAction.CallbackContext context){

        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected == gameObject){
            animator.SetBool("hover", true);
        }
    }


    void Update()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected == gameObject)
        {
            // float if selected
            float t = Time.time;
            float y = Mathf.Sin(t * 4f) * 5f; // slightly stronger effect
            transform.localPosition = startPos + new Vector3(0, y, 0);
        }
        else
        {
            // reset position if not selected
            transform.localPosition = startPos;
        }
    }
    
    //run button animation if mouse is over the button
    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("hover", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("hover", false);
    }
}



/// ORIGINAL BUTTON HOVER FOR REFERENCE ///
/*
public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator animator;

    private Vector3 startPos;
    public float hoverScale = 1.15f;

    void Start()
    {
        animator = GetComponent<Animator>();
        startPos = transform.localPosition;
    }
    void Update()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected == gameObject)
        {
            // float if selected
            float t = Time.time;
            float y = Mathf.Sin(t * 4f) * 5f; // slightly stronger effect
            transform.localPosition = startPos + new Vector3(0, y, 0);

            // pressing space does the like button animation and starts the game
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                animator.SetBool("hover", true);
                FindObjectOfType<MainMenu>().OnStartPressed();
            }
        }
        else
        {
            // reset position if not selected
            transform.localPosition = startPos;
        }
    }

    IEnumerator SpacePressed()
    {
        animator.SetBool("hover", true);
        // small delay so it looks like an animaiton
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("hover", false);

        // actually start the game
        FindObjectOfType<MainMenu>().OnStartPressed();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("hover", true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("hover", false);
    }
}
*///////////////////////////////////