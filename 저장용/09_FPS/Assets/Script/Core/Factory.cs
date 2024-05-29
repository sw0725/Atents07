using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Singltrun<Factory>
{
    BulletHolePool bulletHolePool;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        bulletHolePool = GetComponentInChildren<BulletHolePool>();
        bulletHolePool?.Initialize();
    }

    public BulletHole GetBulletHole() 
    {
        return bulletHolePool?.GetObject();
    }
    public BulletHole GetBulletHole(Vector3 pos, Vector3 normal, Vector3 reflect)       //위치, 총알이 닿을때 파지는 방향
    {
        BulletHole hole = bulletHolePool.GetObject();
        hole.Initialize(pos, normal, reflect);
        return hole;
    }   
}
