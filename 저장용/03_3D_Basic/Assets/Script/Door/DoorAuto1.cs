using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAuto1 : DoorBase
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            float dir = other.transform.position.z - transform.position.z;
            if(dir < 0.0f)
                Open();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Close();
        }
    }
}
