using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemDataEquip : ItemData, IEquipable
{
    [Header("장비 아이템 정보")]
    public GameObject epuipPrefab;

    public virtual EquipType EquipType => EquipType.Weapon;

    public void Equip(GameObject target, InvenSlot slot)
    {
        IEquipTarget equipTarget = target.GetComponent<IEquipTarget>();
        if (equipTarget != null) 
        {
            equipTarget.EquipItem(EquipType, slot);
        }
    }

    public void UnEquip(GameObject target, InvenSlot slot)
    {
        IEquipTarget equipTarget = target.GetComponent<IEquipTarget>();
        if (equipTarget != null)
        {
            equipTarget.UnEquipItem(EquipType);
        }
    }

    public void ToggleEquip(GameObject target, InvenSlot slot)
    {
        IEquipTarget equipTarget = target.GetComponent<IEquipTarget>();
        if (equipTarget != null)
        {
            InvenSlot oldSlot = equipTarget[EquipType];
            if (oldSlot == null)        //장비x
            {
                Equip(target, slot);
            }
            else 
            {
                UnEquip(target, oldSlot);       //원 장비 해제
                if (oldSlot != slot) 
                {
                    Equip(target, slot);
                }
            }
        }
    }
}
