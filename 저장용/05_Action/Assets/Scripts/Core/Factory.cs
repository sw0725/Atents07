using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    //Item= 0,
}

public class Factory : Singltrun<Factory>
{
    public float noisePower = 0.5f;     //노이즈 반지름

    ItemPool itemPool;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        itemPool = GetComponentInChildren<ItemPool>();
        if (itemPool != null) itemPool.Initialize();
    }

    public GameObject GetObject(PoolObjectType type, Vector3? position = null, Vector3? euler = null)
    {
        GameObject result = null;
        //switch (type)
        //{
        //    case PoolObjectType.Slime:
        //        result = slimePool.GetObject(position, euler).gameObject;
        //        break;
        //}

        return result;
    }

    //public Slime GetSlime()
    //{
    //    return slimePool.GetObject();
    //}

    //public Slime GetSlime(Vector3 position, float angle = 0.0f)
    //{
    //    return slimePool.GetObject(position, angle * Vector3.forward);
    //}

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
}