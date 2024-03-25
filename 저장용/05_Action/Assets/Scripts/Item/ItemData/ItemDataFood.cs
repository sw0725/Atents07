using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data Food", menuName = "Scriptable Object/Item Data Food", order = 2)]
public class ItemDataFood : ItemData, IConumable
{
    [Header("음식 아이템 정보")]
    public float tickRegen = 1.0f;
    public float tickInterval = 1.0f;
    public uint totalTickCount =1;

    public void Consume(GameObject target)
    {
        IHealth health = target.GetComponent<IHealth>();
        if (health != null) 
        {
            health.HealthRegernerateByTick(tickRegen, tickInterval, totalTickCount);
        }
    }
}
