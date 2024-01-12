using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType 
{
    PlayerBullet = 0,
    HitEffect,
    Expolsive,
    EnemyWave,
    EnemyAstroid
}

public class Factory : Singltrun<Factory>
{
    BulletPool bullet;
    HitPool Hit;
    Expolsion expolsion;
    EnemyPool enemy;
    AsteroidPool asteroid;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        bullet = GetComponentInChildren<BulletPool>();
        if (bullet != null )
            bullet.Initialized();
        Hit = GetComponentInChildren<HitPool>();
        if (Hit != null)
            Hit.Initialized();
        expolsion = GetComponentInChildren<Expolsion>();
        if (expolsion != null)
            expolsion.Initialized();
        enemy = GetComponentInChildren<EnemyPool>();
        if (enemy != null)
            enemy.Initialized();
        asteroid = GetComponentInChildren<AsteroidPool>();
        if (asteroid != null)
            asteroid.Initialized();
    }

    public GameObject GetObject(PoolObjectType type) 
    {
        GameObject result = null;
        switch (type) 
        {
            case PoolObjectType.PlayerBullet:
                result = bullet.GetObject().gameObject;
                break;
            case PoolObjectType.HitEffect:
                result = Hit.GetObject().gameObject;
                break; 
            case PoolObjectType.Expolsive:
                result = expolsion.GetObject().gameObject;
                break;
            case PoolObjectType.EnemyWave:
                result = enemy.GetObject().gameObject;
                break;
            case PoolObjectType.EnemyAstroid:
                result = asteroid.GetObject().gameObject;
                break;
        }
        return result;
    }

    public GameObject GetObject(PoolObjectType type, Vector3 position) 
    {
        GameObject obj = GetObject(type);
        obj.transform.position = position;

        switch (type)
        {
            case PoolObjectType.EnemyWave:
                Wave enemy = obj.GetComponent<Wave>();
                enemy.SetStartPotition(position);
                break;
        }

        return obj;
    }
    
    public Bullet GetBullet()
    {
        return bullet.GetObject();
    }

    public Bullet GetBullet(Vector3 position) 
    {
        Bullet bulletcomp = bullet.GetObject();
        bulletcomp.transform.position = position;
        return bulletcomp;
    }
    public Expiosion GetHitEffect()
    {
        return Hit.GetObject();
    }

    public Expiosion GetHitEffect(Vector3 position)
    {
        Expiosion Hitcomp = Hit.GetObject();
        Hitcomp.transform.position = position;
        return Hitcomp;
    }
    public Expiosion Getexpolsion()
    {
        return expolsion.GetObject();
    }

    public Expiosion Getexpolsion(Vector3 position)
    {
        Expiosion expcomp = expolsion.GetObject();
        expcomp.transform.position = position;
        return expcomp;
    }
    public Wave GetEnemyWave()
    {
        return enemy.GetObject();
    }

    public Wave GetEnemyWave(Vector3 position)
    {
        Wave enemycomp = enemy.GetObject();
        enemycomp.SetStartPotition(position);
        return enemycomp;
    }

    public Asteroid GetAstroid()
    {
        return asteroid.GetObject();
    }

    public Asteroid GetAstorid(Vector3 position)
    {
        Asteroid asteroidcomp = asteroid.GetObject();
        asteroidcomp.transform.position = position;
        return asteroidcomp;
    }
}
