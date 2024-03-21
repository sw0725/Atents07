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

    public bool noise = true;
    public Transform target;

#if UNITY_EDITOR

    private void Start()
    {
        inventory = new Inventory(GameManager.Instance.Player);
        inventory.AddItem(ItemCode.Ruby);
        inventory.AddItem(ItemCode.Ruby);
        inventory.AddItem(ItemCode.Ruby);
        inventory.AddItem(ItemCode.Ruby);
        inventory.AddItem(ItemCode.Ruby);
        inventory.AddItem(ItemCode.Ruby);
        inventory.AddItem(ItemCode.Ruby);
        inventory.AddItem(ItemCode.Sapphire);
        inventory.AddItem(ItemCode.Sapphire);
        inventory.AddItem(ItemCode.Emerald);
        inventory.AddItem(ItemCode.Emerald);
        inventory.AddItem(ItemCode.Emerald);
        inventory.AddItem(ItemCode.Emerald);
        inventory.MoveItem(2, 3);
        inventory.AddItem(ItemCode.Sapphire, 2);
        inventory.AddItem(ItemCode.Sapphire, 4);
        inventory.AddItem(ItemCode.Sapphire, 4);
        inventory.AddItem(ItemCode.Sapphire, 5);
        inventory.AddItem(ItemCode.Sapphire, 5);
        inventory.AddItem(ItemCode.Sapphire, 5);
        inventory.Test_InventoryPrint();

        inventoryUI.InitializeInventory(inventory);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        inventory.AddItem(code, index);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Factory.Instance.MakeItem(code);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Factory.Instance.MakeItem(code, target.position, noise);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        ItemObject[] itemObjects = FindObjectsByType<ItemObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (ItemObject itemObject in itemObjects)
        {
            ItemData data = itemObject.ItemData;
            itemObject.End();
            Debug.Log($"{data.itemName}È¹µæ");
        }
    }
#endif
}
