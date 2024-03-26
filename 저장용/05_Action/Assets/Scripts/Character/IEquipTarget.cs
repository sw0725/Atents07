using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipTarget
{
    InvenSlot this[EquipType part] { get; }         //장착여부 및 장비 아이템의 슬롯 확인
    void EquipItem(EquipType part, InvenSlot slot);
    void UnEquipItem(EquipType part);

    Transform GetEquipParentTransform(EquipType part);
}
