using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Wave : EnemyBase
{
    public Action giveScore;

    public float amplitude = 3.0f;
    public float frequency = 2.0f;

    float spawnY = 0.0f;
    float totalTime = 0.0f;

    protected override void OnEnable()
    {
        base.OnEnable();
        spawnY = transform.position.y;
        totalTime = 0.0f;
    }

    public void SetStartPotition(Vector3 posotopn) 
    {
        transform.position = posotopn;
        spawnY = posotopn.y;
    }

    protected override void OnMoveUpdate(float deltaTime)
    {
        totalTime += deltaTime * frequency;
        transform.position = new Vector3(transform.position.x - deltaTime * moveSpeed, spawnY + Mathf.Sin(totalTime) * amplitude, 0.0f);
    }
}
