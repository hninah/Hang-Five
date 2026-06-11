using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DemoTimer : MonoBehaviour
{
    [Tooltip("Game automatically returns to menu after time ends")]
    public float maxDemoTime;
    public UnityEvent endDemo = new UnityEvent();

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= maxDemoTime){
            endDemo.Invoke();
        }
    }
}
