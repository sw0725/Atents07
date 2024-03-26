using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data Weapon", menuName = "Scriptable Object/Item Data Weapon", order = 6)]
public class ItemDataWeapon : ItemDataEquip
{
    [Header("무기 아이템 정보")]
    public float attackPower = 30.0f;
}
