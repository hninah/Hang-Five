using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimatedText : MonoBehaviour
{
    public TMP_Text txt;

    // Update is called once per frame
    void Update(){
        //make the instructions fade in and out slightly
        txt.alpha = 1.5f + Mathf.Sin(Time.time * 1.5f);
    }
}
