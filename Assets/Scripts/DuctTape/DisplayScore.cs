using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayScore : MonoBehaviour
{
    public TMP_Text text;

    public void changeTextColor()
    {
        text.color = new Color(255, 255, 255);
    }
}
