using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipTarget
{
    InvenSlot this[EquipType part] { get; }         //�������� �� ��� �������� ���� Ȯ��
    void EquipItem(EquipType part, InvenSlot slot);
    void UnEquipItem(EquipType part);

    Transform GetEquipParentTransform(EquipType part);
}
