using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DigSite : MonoBehaviour
{
    public GameObject dirt;
    public List <GameObject> hiddenObjects = new List<GameObject>();
    public List <GameObject> canvasList = new List<GameObject>();
    public GameObject scroll;
    public List<GameObject> scrollParts = new List<GameObject>();
    int mid = 0, bottom = 1; //parts of the scroll

    public Vector3 movementSpeed;
    public int digTimes = 4;
    public bool visible = false;

    public float closeTime;
    private bool shovelTime = false;
    private DigSiteSpawner digSiteSpawner;
    private int ditSiteSpawnIndex;
    int indexHiddenObject;

    private const float scaleRate = 0.8f;
    private const float minScale = 0.4f;

    public AudioSource foundSound;
    public AudioSource infoSound;

    private void Start()
    {
        indexHiddenObject = UnityEngine.Random.Range(0, hiddenObjects.Count);
        Debug.Log("hiddenindex: " + indexHiddenObject);
        // choosing hidden object
        for (int i = 0; i < hiddenObjects.Count; i++){ hiddenObjects[i].SetActive(false); }
        
        // choosing text
        for (int i = 0; i < canvasList.Count; i++) { canvasList[i].SetActive(false); }
        
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
                    hiddenObjects[indexHiddenObject].GetComponent<Rotate>().isRotating = true;
                    foundSound.Play();
                }
            }
        }
    }
    IEnumerator shrinkObject(GameObject top, GameObject mid)
    {
        GetComponent<AudioSource>().Play();     // scroll sound effect

        float scale_y = mid.transform.localScale.y;
        float pos_z = top.transform.localPosition.z;

        float posRate = -scaleRate * 0.36f;
        while (scale_y > minScale)    //    while the object is larger than desired
        {
            scale_y -= scaleRate * Time.deltaTime;    //    calculate the new scale relative to the rate
            pos_z -= posRate * Time.deltaTime;
            Vector3 scale = mid.transform.localScale;
            scale.y = scale_y;
            mid.transform.localScale = scale;
            Vector3 pos = top.transform.localPosition;
            pos.z = pos_z;
            top.transform.localPosition = pos;
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
        Debug.Log("ho:"+hiddenObjects[indexHiddenObject]);
        hiddenObjects[indexHiddenObject].GetComponent<Rotate>().isRotating = false;


        hiddenObjects[indexHiddenObject].SetActive(false);
        infoSound.Play();
        Debug.Log("closeTime: " + closeTime);
        


        Debug.Log("activating the scroll parts");


        scroll.SetActive(true);
        canvasList[indexHiddenObject].SetActive(true);
        yield return new WaitForSeconds(closeTime * 60);
        Debug.Log("time to close");

        canvasList[indexHiddenObject].SetActive(false);
        StartCoroutine(shrinkObject(scrollParts[bottom], scrollParts[mid]));
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
                else if (hiddenObjects[indexHiddenObject].activeSelf) {
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
        hiddenObjects[indexHiddenObject].SetActive(true);
        visible = true;
        foundSound.Play();
    }

    public List<GameObject> getSpawnPositions()
    {
        return digSiteSpawner.getDigSiteList();
    }
}
