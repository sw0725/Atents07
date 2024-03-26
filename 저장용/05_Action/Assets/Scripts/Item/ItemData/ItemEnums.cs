using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCode : byte    //������ ID
{
    Ruby = 0,
    Emerald,
    Sapphire,
    CopperCoin,
    SliverCoin,
    GoldCoin,
    Apple,
    Bread,
    Beer,
    HealingPotion,
    ManaPotion,
    IronSword,
    SliverSword,
    OldSword
}

public enum ItemSortBy : byte  //������ ���ı���
{
    Code = 0,
    Name,
    Price
}

public enum EquipType :byte
{
    Weapon,
    Shield
}
