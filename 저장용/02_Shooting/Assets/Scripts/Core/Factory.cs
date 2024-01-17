using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType 
{
    PlayerBullet = 0,
    HitEffect,
    Expolsive,
    PowerUp,
    EnemyWave,
    EnemyAstroid,
    EnemyAstroidMini
}

public class Factory : Singltrun<Factory>
{
    BulletPool bullet;
    HitPool Hit;
    Expolsion expolsion;
    PowerUpPool powerUp;
    WavePool enemy;
    AsteroidPool asteroid;
    AsteroidMiniPool asteroidMini;

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
        powerUp = GetComponentInChildren<PowerUpPool>();
        if (powerUp != null)
            powerUp.Initialized();
        enemy = GetComponentInChildren<WavePool>();
        if (enemy != null)
            enemy.Initialized();
        asteroid = GetComponentInChildren<AsteroidPool>();
        if (asteroid != null)
            asteroid.Initialized();
        asteroidMini = GetComponentInChildren<AsteroidMiniPool>();
        if (asteroidMini != null)
            asteroidMini.Initialized();
    }

    public GameObject GetObject(PoolObjectType type, Vector3? position = null, Vector3? euler = null) 
    {
        GameObject result = null;
        switch (type) 
        {
            case PoolObjectType.PlayerBullet:
                result = bullet.GetObject(position, euler).gameObject;
                break;
            case PoolObjectType.HitEffect:
                result = Hit.GetObject(position, euler).gameObject;
                break; 
            case PoolObjectType.Expolsive:
                result = expolsion.GetObject(position, euler).gameObject;
                break;
            case PoolObjectType.PowerUp:
                result = powerUp.GetObject(position, euler).gameObject;
                break;
            case PoolObjectType.EnemyWave:
                result = enemy.GetObject(position, euler).gameObject;
                break;
            case PoolObjectType.EnemyAstroid:
                result = asteroid.GetObject(position, euler).gameObject;
                break;
            case PoolObjectType.EnemyAstroidMini:
                result = asteroidMini.GetObject(position, euler).gameObject;
                break;
        }
        return result;
    }
    
    public Bullet GetBullet()
    {
        return bullet.GetObject();
    }

    public Bullet GetBullet(Vector3 position, float angle = 0.0f) 
    {
        return bullet.GetObject(position, angle * Vector3.forward);
    }
    public Expiosion GetHitEffect()
    {
        return Hit.GetObject();
    }

    public Expiosion GetHitEffect(Vector3 position, float angle = 0.0f)
    {
        return Hit.GetObject(position, angle * Vector3.forward);
    }
    public Expiosion Getexpolsion()
    {
        return expolsion.GetObject();
    }

    public PowerUp GetPowerUp(Vector3 position, float angle = 0.0f)
    {
        return powerUp.GetObject(position, angle * Vector3.forward);
    }
    public PowerUp GetPowerUp()
    {
        return powerUp.GetObject();
    }

    public Expiosion Getexpolsion(Vector3 position, float angle = 0.0f)
    {
        return expolsion.GetObject(position, angle * Vector3.forward);
    }

    public Wave GetEnemyWave()
    {
        return enemy.GetObject();
    }

    public Wave GetEnemyWave(Vector3 position, float angle = 0.0f)
    {
        return enemy.GetObject(position, angle*Vector3.forward);
    }

    public Asteroid GetAstroid()
    {
        return asteroid.GetObject();
    }

    public Asteroid GetAstorid(Vector3 position, float angle = 0.0f)
    {
        return asteroid.GetObject(position, angle * Vector3.forward);
    }
    public AstroidMini GetAstroidMini()
    {
        return asteroidMini.GetObject();
    }

    public AstroidMini GetAstoridMini(Vector3 position, float angle = 0.0f)
    {
        return asteroidMini.GetObject(position, angle*Vector3.forward);
    }
}
