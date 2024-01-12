using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        RecycleObject obj = other.GetComponent<RecycleObject>();
        if (obj != null)
        {
            other.gameObject.SetActive(false);
        }
        else 
        {
            Destroy(other.gameObject);
        }
    }
}
