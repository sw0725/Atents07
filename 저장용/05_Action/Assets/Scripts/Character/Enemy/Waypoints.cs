using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Waypoints : MonoBehaviour
{
    public Transform Current => waypoints[index];

    Transform[] waypoints;
    int index = 0;

    private void Awake()
    {
        waypoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
    }

    public void GoNext()
    {
        index++;
        index %= waypoints.Length;
    }

    void ReturnPatrol()
    {

    }
}
