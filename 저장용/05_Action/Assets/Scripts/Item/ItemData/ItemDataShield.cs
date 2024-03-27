using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data Shield", menuName = "Scriptable Object/Item Data Shield", order = 7)]
public class ItemDataShield : ItemDataEquip
{
    [Header("방패 아이템 정보")]
    public float defencePower = 15.0f;

    public override EquipType EquipType => EquipType.Shield;
}
