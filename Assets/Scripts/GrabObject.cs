using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    public GameObject currentHolding = null; // to detect the current object being holded
    public List<AudioSource> audioSource; // audio sounds when object is picked 

    List<string> tools_idx = new List<string>();
    
    public GameObject digpile;

    private float floorHeight = 2;

    public void Start()
    {
        tools_idx.Add("pickaxe");
        tools_idx.Add("scroll");
        tools_idx.Add("shovel");
        tools_idx.Add("radar");
    }

    public void changeGrabbedObject(GameObject new_object) {
        dropCurrentObject();
        currentHolding = new_object;
        new_object.transform.SetParent(transform);
        Rotate rotation = currentHolding.GetComponent<Rotate>();
        if (rotation != null)
            rotation.isRotating = false;
        if (currentHolding.CompareTag("radar"))
            currentHolding.GetComponent<RadarBeep>().isOn = true;
    }

    public void dropCurrentObject() {
        if (currentHolding != null) //first iteration
        {
            currentHolding.transform.SetParent(null);
            Rotate rotation = currentHolding.GetComponent<Rotate>();
            if (rotation != null)
                rotation.isRotating = true;
            if (currentHolding.CompareTag("radar"))
                currentHolding.GetComponent<RadarBeep>().isOn = false;
            currentHolding.transform.position = new Vector3(
                currentHolding.transform.position.x,
                floorHeight,
                currentHolding.transform.position.z
            );
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.CompareTag("CollideTool"))
        //{
        //Debug.Log("collision with tool ocurred");
        
        // check if tool was grabbed
        for (int i = 0; i < tools_idx.Count; i++)
        {
            if (other.gameObject.CompareTag(tools_idx[i]))
            {
                // grabbing a tool
                Debug.Log("Collision with " + tools_idx[i]);
                changeGrabbedObject(other.gameObject);
                //audioSource[ i ].Play();
            } else if (other.gameObject.CompareTag("glove")) {
                Debug.Log("found another player!");
                // coliding with another player
                GrabObject other_player_object = other.gameObject.GetComponent<GrabObject>();

                if (currentHolding == null || other_player_object.currentHolding == null)
                    return;

                if (currentHolding.CompareTag("scroll") &&
                    other_player_object.currentHolding.CompareTag("digsite")) {
                        Debug.Log("using the scroll!");

                        other_player_object.currentHolding.GetComponent<DigSite>().ShowFossilInfo();
                        other_player_object.dropCurrentObject();
                }
            }
        }

        //}
    }
}