using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipable
{
    EquipType EquipType { get; }                        //��������

    void Equip(GameObject target, InvenSlot slot);      //�������� ��������� ���
    void UnEquip(GameObject target, InvenSlot slot);
    void ToggleEquip(GameObject target, InvenSlot slot);    //��Ȳ������ ����/�����ϱ�
}
