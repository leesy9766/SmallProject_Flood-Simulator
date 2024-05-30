using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("test"))
        {
            Debug.Log("さごごごごごごごごごげごごご");
        }
    }
}
