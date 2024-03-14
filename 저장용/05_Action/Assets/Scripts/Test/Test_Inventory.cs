using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Inventory : TestBase
{
    public ItemCode code = ItemCode.Ruby;
    [Range(0, 5)]
    public uint index = 0;
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
        inventory.AddItem(code);
        inventory.Test_InventoryPrint();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        inventory.AddItem(code, index);
        inventory.Test_InventoryPrint();
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        inventory.RemoveItem(index);
        inventory.Test_InventoryPrint();
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        inventory.ClearSlot(index);
        inventory.Test_InventoryPrint();
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        inventory.ClearInventory();
        inventory.Test_InventoryPrint();
    }
#endif
}