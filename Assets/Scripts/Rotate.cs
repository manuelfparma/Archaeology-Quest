using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float angle = 0f;
    public float rotationSpeed = 20f;
 
    // Update is called once per frame
    void Update()
    {
        angle += rotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }
}
