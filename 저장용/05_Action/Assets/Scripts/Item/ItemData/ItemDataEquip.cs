using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemDataEquip : ItemData, IEquipable
{
    [Header("��� ������ ����")]
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
            if (oldSlot == null)        //���x
            {
                Equip(target, slot);
            }
            else 
            {
                UnEquip(target, oldSlot);       //�� ��� ����
                if (oldSlot != slot) 
                {
                    Equip(target, slot);
                }
            }
        }
    }
}
