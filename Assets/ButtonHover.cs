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

    void Start()
    {
        animator = GetComponent<Animator>();
        startPos = transform.localPosition;
    }

    void Update()
    {
        float t = Time.time;

        // slightly going up and down
        float y = Mathf.Sin(t * 2f) * 3f;
        transform.localPosition = startPos + new Vector3(0, y, 0);
        // pressing space does the like button animation and starts the game
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StartCoroutine(SpacePressed());
        }
    }
    IEnumerator SpacePressed()
    {
        animator.SetBool("hover", true);
        // small delay so it looks like an animaiton
        yield return new WaitForSeconds(0.15f);
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
