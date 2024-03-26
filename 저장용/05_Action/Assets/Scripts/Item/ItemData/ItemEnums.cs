using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCode : byte    //아이템 ID
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

public enum ItemSortBy : byte  //아이템 정렬기준
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
