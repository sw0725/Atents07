using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;            //������ �κ��丮

    InvenSlotUI[] slotUIs;      //���� ���� ui
    TempSlotUI tempSlotUI;

    private void Awake()
    {
        Transform c = transform.GetChild(0);
        slotUIs = c.GetComponentsInChildren<InvenSlotUI>();

        tempSlotUI = GetComponentInChildren<TempSlotUI>();
    }
    public void InitializeInventory(Inventory playerInventory)  //�ʱ�ȭ
    {
        inven = playerInventory;

        for (uint i = 0; i < slotUIs.Length; i++) 
        {
            slotUIs[i].InitalizeSlot(inven[i]);                 //���� ���� �ʱ�ȭ(����)
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
