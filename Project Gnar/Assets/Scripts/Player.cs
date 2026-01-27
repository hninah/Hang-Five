using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float playerSpeed;

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        transform.position += Vector3.up * verticalInput * playerSpeed * Time.deltaTime;

    }

}
