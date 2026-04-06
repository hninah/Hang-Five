using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BeginGameText : MonoBehaviour
{
    float timer = 5.0f;
    public TMP_Text text;

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
