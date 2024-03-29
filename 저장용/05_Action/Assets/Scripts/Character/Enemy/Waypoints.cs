using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Waypoints : MonoBehaviour
{
    public Vector3 NextTarget => children[index].position;

    Transform[] children;
    int index = 0;

    private void Awake()
    {
        children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }
    }

    public void StepNextWaypoint()
    {
        index++;
        index %= children.Length;
    }
}
