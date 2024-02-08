using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBase : WayPointUser
{
    public Action<Vector3> onMove;
    
    float saveSpeed = 5.0f;

    protected override void OnMove()
    {
        base.OnMove();
        onMove?.Invoke(moveDelta);
    }

    protected override void OnArrived()
    {
        base.OnArrived();
        moveSpeed = 0.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        moveSpeed = saveSpeed;
    }
}
