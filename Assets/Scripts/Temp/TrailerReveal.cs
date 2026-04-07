using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class TrailerReveal : MonoBehaviour
{
    [SerializeField] Image title;
    [SerializeField] TMP_Text dateText;
    [SerializeField] TMP_Text creatorText;
    [SerializeField] Image itchLogo;

    [SerializeField] float callTimer = 3.0f;
    [SerializeField] float secondTimer = 1.5f;
    bool callMade = false;
    bool secondMade = false;

    public UnityEvent removeCall = new UnityEvent();
    public UnityEvent secondCall = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (callMade && secondMade) return;

        callTimer -= Time.deltaTime;

        if (callTimer <= 0.0f && !callMade)
        {
            callMade = true;
            removeCall.Invoke();
        }

        if (!callMade) return;

        secondTimer -= Time.deltaTime;
        if (secondTimer <= 0.0f && !secondMade)
        {
            secondMade = true;
            secondCall.Invoke();
        }
    }
}
