using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;            //보여줄 인벤토리

    InvenSlotUI[] slotUIs;      //안의 슬롯 ui
    TempSlotUI tempSlotUI;

    private void Awake()
    {
        Transform c = transform.GetChild(0);
        slotUIs = c.GetComponentsInChildren<InvenSlotUI>();

        tempSlotUI = GetComponentInChildren<TempSlotUI>();
    }
    public void InitializeInventory(Inventory playerInventory)  //초기화
    {
        inven = playerInventory;

        for (uint i = 0; i < slotUIs.Length; i++) 
        {
            slotUIs[i].InitalizeSlot(inven[i]);                 //안의 슬롯 초기화(연결)
            slotUIs[i].onDragBegin += OnItemMoveBegin;
            slotUIs[i].onDragEnd += OnItemMoveEnd;
        }

        tempSlotUI.InitalizeSlot(inven.TempSlot);
    }

    private void OnItemMoveBegin(uint index)
    {
        inven.MoveItem(index, tempSlotUI.Index);
    }

    private void OnItemMoveEnd(uint index, bool isSucess)
    {
        inven.MoveItem(tempSlotUI.Index, index);
    }
}
