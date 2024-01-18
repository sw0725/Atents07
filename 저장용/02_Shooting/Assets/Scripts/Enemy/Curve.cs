using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve : EnemyBase
{
    [Header("Curve data")]

    public float rotateSpeed = 10.0f;

    float curveDirection = 1.0f; // 1or-1

    protected override void OnMoveUpdate(float deltaTime)
    {
        base.OnMoveUpdate(deltaTime);
        transform.Rotate(deltaTime * rotateSpeed * curveDirection * Vector3.forward);
    }

    public void RefeashRotate()
    {
        if (transform.position.y < 0)
        {
            curveDirection = -1.0f;
        }
        else 
        {
            curveDirection = 1.0f;
        }
    }
}
