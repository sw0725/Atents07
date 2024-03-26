using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data Weapon", menuName = "Scriptable Object/Item Data Weapon", order = 6)]
public class ItemDataWeapon : ItemDataEquip
{
    [Header("���� ������ ����")]
    public float attackPower = 30.0f;
}
