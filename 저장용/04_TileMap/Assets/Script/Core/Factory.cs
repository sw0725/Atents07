using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    None = 0,
}

public class Factory : Singleton<Factory>
{
    //BulletPool bulletPool;

    //protected override void OnInitialize()
    //{
    //    base.OnInitialize();

    //    bulletPool = GetComponentInChildren<BulletPool>();
    //    if (bulletPool != null) bulletPool.Initialize();
    //}
 
    //public GameObject GetObject(PoolObjectType type, Vector3? position = null, Vector3? euler = null)
    //{
    //    GameObject result = null;
    //    switch (type)
    //    {
    //        case PoolObjectType.Bullet:
    //            result = bulletPool.GetObject(position, euler).gameObject;
    //            break;
    //    }

    //    return result;
    //}

    //public Bullet GetBullet()
    //{
    //    return bulletPool.GetObject();
    //}

    //public Bullet GetBullet(Vector3 position, float angle = 0.0f)
    //{
    //    return bulletPool.GetObject(position, angle * Vector3.forward);
    //}
}