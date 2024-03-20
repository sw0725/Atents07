using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ItemDrop : TestBase
{
    public InventoryUI inventoryUI;
    Inventory inventory;

    public ItemCode code = ItemCode.Ruby;
    [Range(0, 5)]
    public uint index = 0;
    [Range(0, 5)]
    public uint index2 = 0;

    public ItemSortBy sort = ItemSortBy.Code;
    public bool isAssanding = true;

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

        inventoryUI.InitializeInventory(inventory);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
    }
#endif
}
