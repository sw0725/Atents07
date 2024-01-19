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
    EnemyAstroidMini,
    EnemyBouns,
    EnemyCurve,
    EnemyBoss,
    EnemyBossBullet,
    EnemyBossMissle,
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
    BounsPool bouns;
    CurvePool curve;
    BossPool boss;
    BossBulletPool bossBullet;
    BossMisslePool bossMissle;

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
        bouns = GetComponentInChildren<BounsPool>();
        if (bouns != null)
            bouns.Initialized();
        curve = GetComponentInChildren<CurvePool>();
        if (curve != null)
            curve.Initialized();
        boss = GetComponentInChildren<BossPool>();
        if (boss != null)
            boss.Initialized();
        bossBullet = GetComponentInChildren<BossBulletPool>();
        if (bossBullet != null)
            bossBullet.Initialized();
        bossMissle = GetComponentInChildren<BossMisslePool>();
        if (bossMissle != null)
            bossMissle.Initialized();
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
            case PoolObjectType.EnemyBouns:
                result = bouns.GetObject(position, euler).gameObject;
                break;
            case PoolObjectType.EnemyCurve:
                result = curve.GetObject(position, euler).gameObject;
                break;
            case PoolObjectType.EnemyBoss:
                result = boss.GetObject(position, euler).gameObject;
                break;
            case PoolObjectType.EnemyBossBullet:
                result = bossBullet.GetObject(position, euler).gameObject;
                break;
            case PoolObjectType.EnemyBossMissle:
                result = bossMissle.GetObject(position, euler).gameObject;
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
    public BossBullet GetBossBullet()
    {
        return bossBullet.GetObject();
    }
    public BossBullet GetBossBullet(Vector3 position, float angle = 0.0f)
    {
        return bossBullet.GetObject(position, angle * Vector3.forward);
    }

    public BossMissle GetBossMissle(Vector3 position, float angle = 0.0f)
    {
        return bossMissle.GetObject(position, angle * Vector3.forward);
    }
    public BossMissle GetBossMissle()
    {
        return bossMissle.GetObject();
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

    public Expiosion Getexpolsion(Vector3 position, float angle = 0.0f)
    {
        return expolsion.GetObject(position, angle * Vector3.forward);
    }


    public PowerUp GetPowerUp(Vector3 position, float angle = 0.0f)
    {
        return powerUp.GetObject(position, angle * Vector3.forward);
    }
    public PowerUp GetPowerUp()
    {
        return powerUp.GetObject();
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
    public Bouns GetBouns()
    {
        return bouns.GetObject();
    }

    public Bouns GetBouns(Vector3 position, float angle = 0.0f)
    {
        return bouns.GetObject(position, angle * Vector3.forward);
    }
    public Curve GetCurve()
    {
        return curve.GetObject();
    }

    public Curve GetCurve(Vector3 position, float angle = 0.0f)
    {
        return curve.GetObject(position, angle * Vector3.forward);
    }
    public Boss GetBoss()
    {
        return boss.GetObject();
    }

    public Boss GetBoss(Vector3 position, float angle = 0.0f)
    {
        return boss.GetObject(position, angle * Vector3.forward);
    }
}
