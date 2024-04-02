using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Factory : Singltrun<Factory>
{
    public float noisePower = 0.5f;     //노이즈 반지름

    ItemPool itemPool;
    HitEffectPool hitPool;
    EnemyPool enemyPool;
    DamegeTextPool damegePool;
    protected override void OnInitialize()
    {
        base.OnInitialize();

        itemPool = GetComponentInChildren<ItemPool>();
        if (itemPool != null) itemPool.Initialize();

        hitPool = GetComponentInChildren<HitEffectPool>();
        if (hitPool != null) hitPool.Initialize();

        enemyPool = GetComponentInChildren<EnemyPool>();
        if (enemyPool != null) enemyPool.Initialize();

        damegePool = GetComponentInChildren<DamegeTextPool>();
        if (damegePool != null) damegePool.Initialize();
    }

    public Enemy GetEnemy() 
    {
        return enemyPool.GetObject();
    }

    public Enemy GetEnemy(Vector3 position, float angle = 0.0f)
    {
        return enemyPool.GetObject(position, angle * Vector3.forward);
    }

    public Enemy GetEnemy(int index, Vector3 position, float angle = 0.0f)
    {
        return enemyPool.GetObject(index, position, angle * Vector3.forward);
    }

    public GameObject GetHitEffect(Vector3? position)
    {
        return hitPool.GetObject(position).gameObject;
    }

    public GameObject MakeItem(ItemCode code) 
    {
        ItemData data = GameManager.Instance.ItemData[code];
        ItemObject obj = itemPool.GetObject();
        obj.ItemData = data;

        return obj.gameObject;
    }

    public GameObject MakeItem(ItemCode code, Vector3 position, bool useNoise = false)
    {
        GameObject obj = MakeItem(code);
        Vector3 noise = Vector3.zero;
        if (useNoise) 
        {
            Vector2 rand = Random.insideUnitCircle* noisePower;
            noise.x = rand.x;
            noise.z = rand.y;
        }
        obj.transform.position = position + noise;

        return obj.gameObject;
    }

    public GameObject[] MakeItems(ItemCode code, uint count) 
    {
        GameObject[] items = new GameObject[count];
        for (int i = 0; i < count; i++) 
        {
            items[i] = MakeItem(code);
        }
        return items;
    }

    public GameObject[] MakeItems(ItemCode code, uint count, Vector3 position, bool useNoise = false)
    {
        GameObject[] items = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            items[i] = MakeItem(code, position, useNoise);
        }
        return items;
    }

    public GameObject GetDamegeText(int damege, Vector3? position)
    {
        return damegePool.GetObject(damege, position);
    }
}