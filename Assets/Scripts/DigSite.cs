using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigSite : MonoBehaviour
{
    public GameObject dirt;
    public GameObject fossil;
    public GameObject scroll;
    public List<GameObject> scrollParts = new List<GameObject>();
    int top = 0, mid = 1, bottom = 2; //parts of the scroll
    public GameObject information;
    public Vector3 movementSpeed;
    public int digTimes = 4;
    public bool visible = false;

    public float closeTime = 3;
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
    IEnumerator shrinkObject(GameObject g, float scaleRate, float minScale)
    {
        float scale_z = g.transform.localScale.z;
        while (scale_z > minScale)    //    while the object is larger than desired
        {
            scale_z -= scaleRate * Time.deltaTime;    //    calculate the new scale relative to the rate
            Vector3 scale = g.transform.localScale;
            scale.z = scale_z;
            g.transform.localScale.Set(g.transform.localScale.x, g.transform.localScale.y,scale_z);
            yield return null;    //    wait a frame
        }
        yield break;
    }

    GameObject GetChildWithName(GameObject obj, string name)
    {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            return childTrans.gameObject;
        }
        else
        {
            return null;
        }
    }

    private IEnumerator DespawnScroll()
    {
        Debug.Log("start scroll despawn time");
        fossil.GetComponent<Rotate>().isRotating = false;
        information.SetActive(true);
        
        
        fossil.SetActive(false);
        GetComponent<AudioSource>().Play();     // scroll sound effect
        Debug.Log("closeTime: " + closeTime);
        Debug.Log("SM Ls x: " + scrollParts[mid].transform.localScale.x);
        Debug.Log("SM Ls y: " + scrollParts[mid].transform.localScale.y);
        Debug.Log("SM Ls z: " + scrollParts[mid].transform.localScale.z);

        scrollParts[mid].transform.localScale.Set(.1f * scrollParts[mid].transform.localScale.x, .1f * scrollParts[mid].transform.localScale.y, .1f * scrollParts[mid].transform.localScale.z);

        Debug.Log("activating the scroll parts");
        scroll.SetActive(true);

        yield return new WaitForSeconds(closeTime);
        Debug.Log("time to close");

        //StartCoroutine(shrinkObject(scrollMid,0.1f,1));
        
        digSiteSpawner.RemoveDigSiteFromList(gameObject);
        Destroy(gameObject);
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
