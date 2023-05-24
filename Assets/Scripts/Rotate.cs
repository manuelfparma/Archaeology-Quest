using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float angle = 0f;
    public float rotationSpeed = 20f;
    public bool isRotating = true;
    public bool allAxis = false;
 
    // Update is called once per frame
    void Update()
    {
        if (isRotating) {
            angle += rotationSpeed * Time.deltaTime;
            if (allAxis) 
                transform.rotation = Quaternion.Euler(angle, angle, angle);
            else
                transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
        }
    }
}
