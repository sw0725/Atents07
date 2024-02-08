using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IAive aive = other.GetComponent<IAive>();
        if(aive != null) 
        {
            aive.Die();
        }
    }
}
