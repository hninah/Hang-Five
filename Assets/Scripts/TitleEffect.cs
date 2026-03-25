using UnityEngine;
using UnityEngine.UI;

public class TitleEffect : MonoBehaviour
{
    // how fast and by how much the pulsating happens
    public float speed = 2f;
    public float scaleAmount = 0.05f;
    // how far it moves up and down and how much it rotates
    public float floatStrength = 5f;
    public float tiltStrength = 2f;

    private Vector3 startScale;
    private Vector3 startPos;

    private Image img;
    // tracks how long the intro has been running and whether or not its finished
    float introTimer = 0f;
    bool started = false;

    void Start()
    {
        // save the original size and position
        startScale = transform.localScale;
        startPos = transform.localPosition;
        img = GetComponent<Image>();
        // start at 10% of original size so it can like get bigger
        transform.localScale = startScale * 0.1f;
    }

    void Update()
    {
        float t = Time.time;

        // intro like pop-up
        if (!started)
        {
            introTimer += Time.deltaTime * 2f;
            // gradually scales from 0.1 to 1.1 so there's that growing effect 
            float s = Mathf.Lerp(0.1f, 1.1f, introTimer);
            transform.localScale = startScale * s;
            // when the like growing thing is done just reset it back original scale
            if (introTimer > 1f)
            {
                started = true;
                transform.localScale = startScale;
            }
            return;
        }

        // using sin wave to go big and then go small
        float scale = 1 + Mathf.Sin(t * speed) * scaleAmount;
        transform.localScale = startScale * scale;
        // using sin wave to go up and down
        float y = Mathf.Sin(t * 1.5f) * floatStrength;
        transform.localPosition = startPos + new Vector3(0, y, 0);
        // using sin wave to rotate left and right a little
        float rot = Mathf.Sin(t * 1.2f) * tiltStrength;
        transform.rotation = Quaternion.Euler(0, 0, rot);
    }
}