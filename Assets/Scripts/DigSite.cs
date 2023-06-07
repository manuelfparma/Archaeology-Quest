using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DigSite : MonoBehaviour
{
    public GameObject dirt;
    public GameObject fossil;
    public GameObject scroll;
    public List<GameObject> scrollParts = new List<GameObject>();
    int top = 0, mid = 1, bottom = 2; //parts of the scroll

    public GameObject information;
    public GameObject miniObject;
    public Vector3 movementSpeed;
    public int digTimes = 4;
    public bool visible = false;

    public float closeTime = 60;
    private bool shovelTime = false;
    private DigSiteSpawner digSiteSpawner;
    private int ditSiteSpawnIndex;

   
    GameObject scrollTop, scrollMid, scrollBottom;
    private void Start()
    {

    }
    private void DigDirt(GameObject tool)
    {
        if (dirt != null && dirt.activeSelf)
        {
            if (shovelTime ? tool.CompareTag("shovel") : tool.CompareTag("pickaxe")) {
                shovelTime = !shovelTime;
                digTimes--;

                // play sound effect
                tool.GetComponent<AudioSource>().Play();

                // Translate only the dirt pile
                dirt.transform.Translate(movementSpeed);
                if (digTimes == 0) {
                    dirt.SetActive(false);
                    fossil.GetComponent<Rotate>().isRotating = true;
                }
            }
        }
    }
    IEnumerator shrinkObject(GameObject top, GameObject mid, float scaleRate, float minScale)
    {
        float scale_y = top.transform.localScale.y;
        float pos_z = mid.transform.localPosition.z;

        float posRate = scaleRate * 0.1f;
        while (scale_y > minScale)    //    while the object is larger than desired
        {
            scale_y -= scaleRate * Time.deltaTime;    //    calculate the new scale relative to the rate
            pos_z -= posRate * Time.deltaTime;
            Vector3 scale = top.transform.localScale;
            scale.y = scale_y;
            top.transform.localScale = scale;
            Vector3 pos = mid.transform.localPosition;
            pos.z = pos_z;
            mid.transform.localPosition = pos;
            Debug.Log("newscale: " +  scale);
            yield return null;    //    wait a frame
        }
        digSiteSpawner.RemoveDigSiteFromList(gameObject);
        Destroy(gameObject);
        yield break; 
    }

    private IEnumerator DespawnScroll()
    {
        Debug.Log("start scroll despawn time");
        fossil.GetComponent<Rotate>().isRotating = false;
        information.SetActive(true);
        
        
        fossil.SetActive(false);
        GetComponent<AudioSource>().Play();     // scroll sound effect
        Debug.Log("closeTime: " + closeTime);
        


        Debug.Log("activating the scroll parts");


        scroll.SetActive(true);
        yield return new WaitForSeconds(closeTime);
        Debug.Log("time to close");
        miniObject.SetActive(false);
        information.SetActive(false);
        StartCoroutine(shrinkObject(scrollParts[bottom], scrollParts[mid], 0.1f, 0.1f));

        //StartCoroutine(shrinkObject(scrollMid,0.1f,1));

        
    }
    
    public void ShowFossilInfo() {
        StartCoroutine(DespawnScroll());
    }
    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("glove")) {
            GameObject tool = other.GetComponent<GrabObject>().currentHolding;
            if (tool != null) {
                if (digTimes > 0)
                    DigDirt(tool);
                else if (fossil.activeSelf) {
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

    public void makeVisible() {
        dirt.SetActive(true);
        fossil.SetActive(true);
        visible = true;
    }
}
