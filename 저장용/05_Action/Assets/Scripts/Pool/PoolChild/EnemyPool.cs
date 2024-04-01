using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : ObjectPool<Enemy>
{
    public Waypoints[] waypoints;                           //반드시 하나필수

    private void Awake()
    {
        Transform c = transform.GetChild(0);
        waypoints = c.GetComponentsInChildren<Waypoints>();
    }

    public Enemy GetObject(int index, Vector3? position = null, Vector3? eulerAngle = null) 
    {
        Enemy enemy = GetObject(position, eulerAngle);
        enemy.waypoints = waypoints[index];

        return enemy;
    }

    protected override void OnGenerateObject(Enemy comp)
    {
        comp.waypoints = waypoints[0];                      //디폴트
    }
}
