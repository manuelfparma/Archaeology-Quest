using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTool : MonoBehaviour
{
    //public Transform player;
    private Collider currentHolding = null;

    public void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.CompareTag("CollideTool"))
        //{
        Debug.Log("collision with tool ocurred");
        if (currentHolding != null) //first iteration
        {
            currentHolding.transform.SetParent(null);
        }
        currentHolding = other;
        other.transform.SetParent(transform);
        //}
    }
}
