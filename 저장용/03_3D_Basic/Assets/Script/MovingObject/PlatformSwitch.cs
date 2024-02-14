using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSwitch : PlatformBase, IInteracable
{
    bool isMoving = false;

    public bool CanUse => true;

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

    public void Use()
    {
        isMoving = true;
    }
}
