using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Singltrun<Factory>
{
    BulletHolePool bulletHolePool;
    AssaultRiflePool assaultRiflePool;
    ShotGunPool shotGunPool;
    HealPackPool healPackPool;
    protected override void OnInitialize()
    {
        base.OnInitialize();

        bulletHolePool = GetComponentInChildren<BulletHolePool>();
        bulletHolePool?.Initialize();
        assaultRiflePool = GetComponentInChildren<AssaultRiflePool>();
        assaultRiflePool?.Initialize();
        shotGunPool = GetComponentInChildren<ShotGunPool>();
        shotGunPool?.Initialize();
        healPackPool = GetComponentInChildren<HealPackPool>();
        healPackPool?.Initialize();
    }

    public BulletHole GetBulletHole() 
    {
        return bulletHolePool?.GetObject();
    }
    public BulletHole GetBulletHole(Vector3 pos, Vector3 normal, Vector3 reflect)       //생성위치, 생성될 면의 노말, 반사 방향       //위치, 총알이 닿을때 파지는 방향
    {
        BulletHole hole = bulletHolePool.GetObject();
        hole.Initialize(pos, normal, reflect);
        return hole;
    }

    public GunItem GetAssaultRifle(Vector3 pos)
    {
        GunItem item = assaultRiflePool?.GetObject();
        item.transform.position = pos;
        return item;
    }

    public GunItem GetShotGun(Vector3 pos)
    {
        GunItem item = shotGunPool?.GetObject();
        item.transform.position = pos;
        return item;
    }
    public HealItem GetHealPack(Vector3 pos)
    {
        HealItem item = healPackPool?.GetObject();
        item.transform.position = pos;
        return item;
    }
}
