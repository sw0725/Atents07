using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Inventory1 : TestBase
{
    public ItemCode code = ItemCode.Ruby;
    [Range(0, 5)]
    public uint index = 0;

    [Range(0, 5)]
    public uint to = 0;

    public ItemSortBy sort = ItemSortBy.Code;
    public bool isAssanding = true;

    Inventory inventory;

#if UNITY_EDITOR
    private void Start()
    {
        inventory = new Inventory(null);
        inventory.AddItem(ItemCode.Ruby);
        inventory.AddItem(ItemCode.Sapphire);
        inventory.AddItem(ItemCode.Sapphire);
        inventory.AddItem(ItemCode.Emerald);
        inventory.AddItem(ItemCode.Emerald);
        inventory.AddItem(ItemCode.Emerald);
        inventory.MoveItem(2, 3);
        inventory.Test_InventoryPrint();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        inventory.MoveItem(index, to);
        inventory.Test_InventoryPrint();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        inventory.SplitItem(index, to);
        inventory.Test_InventoryPrint();
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        inventory.SlotSorting(sort, isAssanding);
        inventory.Test_InventoryPrint();
    }
#endif
}