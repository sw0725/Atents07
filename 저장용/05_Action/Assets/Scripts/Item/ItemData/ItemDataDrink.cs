using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data Drink", menuName = "Scriptable Object/Item Data Drink", order = 3)]
public class ItemDataDrink : ItemData, IConumable
{
    [Header("���� ������ ����")]
    public float totalRegen;
    public float duration;

    public void Consume(GameObject target)
    {
        IMana mana = target.GetComponent<IMana>();
        if (mana != null)
        {
            mana.ManaRegernerate(totalRegen, duration);
        }
    }
}
