using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data HealingPotion", menuName = "Scriptable Object/Item Data HealingPotion", order = 4)]
public class ItemDataHealingPotion : ItemData, IUsable
{
    [Header("힐링 포션 정보")]
    public float healRatio = 0.3f;      //최대hp 비례 회복
    public float tickRegen = 4.0f;
    public float tickInterval = 0.4f;
    public uint totalTickCount =5;

    public bool Use(GameObject target)
    {
        bool result = false;
        IHealth health = target.GetComponent<IHealth>();
        if (health != null)
        {
            if (health.HP < health.MaxHP)
            {
                health.HP += health.MaxHP * healRatio;
                health.HealthRegernerateByTick(tickRegen, tickInterval, totalTickCount);
                result = true;
            }
            else 
            {
                Debug.Log("풀피");
            }
        }
        return result;
    }
}
