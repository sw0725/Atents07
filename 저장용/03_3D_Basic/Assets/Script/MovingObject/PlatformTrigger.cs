using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : PlatformBase
{
    bool isMoving = false;

    private void Start()
    {
        Target = targetWaypoints.GetNextPoint();
    }

    protected override void OnMove()
    {
        if (isMoving)
        {
            base.OnMove();
        }
    }

    protected override void OnArrived()
    {
        isMoving = false;
        base.OnArrived();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            isMoving |= true;
        }
    }
}