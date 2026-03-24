using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextParticle : MonoBehaviour
{
    [SerializeField] float ySpeed = 1.0f;
    [SerializeField] float fadeSpeed = 1.0f;
    [SerializeField] TMP_Text particleText;
    public string Text { get { return particleText.text; } set { particleText.text = value; } }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0.0f, ySpeed * Time.deltaTime, 0.0f);
        particleText.alpha = Mathf.Max(0.0f, particleText.alpha - fadeSpeed * Time.deltaTime);

        if (particleText.alpha <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
