using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTool : MonoBehaviour
{
    public GameObject currentHolding = null; // to detect the current object being holded
    public List<AudioSource> audioSource; // audio sounds when object is picked 

    List<string> tools_idx = new List<string>();

    public void Start()
    {
        tools_idx.Add("pickaxe");
        tools_idx.Add("scroll");
        tools_idx.Add("shovel");
        tools_idx.Add("radar");
    }

    private void changeGrabbedObject(Collider new_object) {
        if (currentHolding != null) //first iteration
        {
            currentHolding.transform.SetParent(null);
            currentHolding.GetComponent<Rotate>().isRotating = true;
        }
        currentHolding = new_object.gameObject;
        new_object.transform.SetParent(transform);
        currentHolding.GetComponent<Rotate>().isRotating = false;
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
                Debug.Log("Collision with" + tools_idx[i]);
                changeGrabbedObject(other);
                //audioSource[ i ].Play();
            }
        }

        //}
    }
}