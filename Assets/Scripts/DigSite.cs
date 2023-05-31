using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigSite : MonoBehaviour
{
    public GameObject dirt;
    public GameObject fossil;
    public GameObject scroll;
    public GameObject information;
    public Vector3 movementSpeed;
    public int digTimes = 4;
    private bool shovelTime = false;
    private DigSiteSpawner digSiteSpawner;
    private int ditSiteSpawnIndex;

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
    private IEnumerator DespawnScroll()
    {
        yield return new WaitForSeconds(7);
        scroll.SetActive(false); 
    }
    public void ShowFossilInfo() {
        fossil.GetComponent<Rotate>().isRotating = false;
        information.SetActive(true);
        scroll.SetActive(true);
        StartCoroutine(DespawnScroll());
        fossil.SetActive(false);

        digSiteSpawner.RemoveDigSiteFromList(gameObject);
        Destroy(gameObject);

    }
    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("glove")) {
            GameObject tool = other.GetComponent<GrabObject>().currentHolding;
            if (tool != null) {
                if (digTimes > 0)
                    DigDirt(tool);
                else {
                    other.gameObject.GetComponent<GrabObject>().changeGrabbedObject(this.gameObject);
                }
            }
        }
    }

    public void SetSpawner(DigSiteSpawner spawner, int index)
    {
        digSiteSpawner = spawner;
        ditSiteSpawnIndex = index;
    }
}
