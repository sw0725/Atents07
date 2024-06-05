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
    public BulletHole GetBulletHole(Vector3 pos, Vector3 normal, Vector3 reflect)       //������ġ, ������ ���� �븻, �ݻ� ����       //��ġ, �Ѿ��� ������ ������ ����
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
