using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipable
{
    EquipType EquipType { get; }                        //장착부위

    void Equip(GameObject target, InvenSlot slot);      //누구에게 어느슬롯의 장비를
    void UnEquip(GameObject target, InvenSlot slot);
    void ToggleEquip(GameObject target, InvenSlot slot);    //상황에따라 장착/해제하기
}
