using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    const int Default_Inventory_Size = 6;
    InvenSlot[] slots;

    public InvenSlot this[uint index] => slots[index];
    int SlotCount => slots.Length;

    InvenSlot tempSlot;
    uint tempSlotIndex = 1111111;
    public InvenSlot TempSlot => tempSlot;

    ItemDataManager ItemDataManager;
    Player owner;
    public Player Owner => owner;
}
