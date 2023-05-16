using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTool : MonoBehaviour
{
    private Collider currentHolding = null; // to detect the current object being holded
    public List<AudioSource> audioSource; // audio sounds when object is picked 

    List<string> tools_idx = new List<string>();

    public void Start()
    {
        tools_idx.Add("pickaxe");
        tools_idx.Add("scroll");
        tools_idx.Add("shovel");
        tools_idx.Add("radar");
    }

    public void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.CompareTag("CollideTool"))
        //{
        //Debug.Log("collision with tool ocurred");
        if (currentHolding != null) //first iteration
        {
            currentHolding.transform.SetParent(null);
        }
        currentHolding = other;
        other.transform.SetParent(transform);

        for (int i = 0; i < tools_idx.Count; i++)
        {
            if (other.gameObject.CompareTag(tools_idx[i]))
            {
                Debug.Log("Collision with" + tools_idx[i]);
                Rotate rotationProp = other.gameObject.GetComponent<Rotate>();
                rotationProp.isRotating = false;
                //audioSource[ i ].Play();
            }
        }


        //}
    }
}