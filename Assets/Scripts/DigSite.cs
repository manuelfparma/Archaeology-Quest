using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigSite : MonoBehaviour
{
    public GameObject dirt;
    public GameObject fossil;
    public Vector3 movementSpeed;
    private int digTimes = 4;
    private bool shovelTime = false;

    private void DigDirt(GameObject tool)
    {
        if (dirt != null && dirt.activeSelf)
        {
            if (shovelTime ? tool.CompareTag("shovel") : tool.CompareTag("pickaxe")) {
                shovelTime = !shovelTime;
                digTimes--;

                // Translate only the dirt pile
                dirt.transform.Translate(movementSpeed);
                if (digTimes == 0) {
                    dirt.SetActive(false);
                    fossil.GetComponent<Rotate>().isRotating = true;
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("glove")) {
            GameObject tool = other.GetComponent<GrabTool>().currentHolding;
            if (tool != null && digTimes > 0) {
                DigDirt(tool);
            }
        }
    }
}
