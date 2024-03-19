using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;            //������ �κ��丮

    InvenSlotUI[] slotUIs;      //���� ���� ui
    TempSlotUI tempSlotUI;
    DetaiInfo detaiInfo;

    private void Awake()
    {
        Transform c = transform.GetChild(0);
        slotUIs = c.GetComponentsInChildren<InvenSlotUI>();

        tempSlotUI = GetComponentInChildren<TempSlotUI>();
        detaiInfo = GetComponentInChildren<DetaiInfo>();
    }
    public void InitializeInventory(Inventory playerInventory)  //�ʱ�ȭ
    {
        inven = playerInventory;

        for (uint i = 0; i < slotUIs.Length; i++) 
        {
            slotUIs[i].InitalizeSlot(inven[i]);                 //���� ���� �ʱ�ȭ(����)
            slotUIs[i].onDragBegin += OnItemMoveBegin;
            slotUIs[i].onDragEnd += OnItemMoveEnd;
            slotUIs[i].onClick += OnSlotClick;
            slotUIs[i].onPointerEnter += OnItemDetailOn;
            slotUIs[i].onPointerExit += OnItemDetailOff;
            slotUIs[i].onPointerMove += OnSlotPointerMove;
        }

        tempSlotUI.InitalizeSlot(inven.TempSlot);
    }

    private void OnItemMoveBegin(uint index)
    {
        detaiInfo.IsPause = true;               //�巡���Ҷ� �Ⱥ��̰�
        inven.MoveItem(index, tempSlotUI.Index);
        tempSlotUI.Open();
    }

    private void OnItemMoveEnd(uint index, bool isSlotEnd)
    {                               //���Կ��� �巡�� ������ ������ �ƴϸ� ���۽���
        //uint finalIndex = index;
        //if (isSlotEnd) 
        //{
        //    if (inven.FindEmptySlot(out uint emptySlotIndex))
        //    {
        //        finalIndex = emptySlotIndex;
        //    }
        //    else 
        //    {
        //        //�ٴڿ� ���
        //        Debug.Log("������ ��� �ʿ�");
        //        return;
        //    }
        //}
        inven.MoveItem(tempSlotUI.Index, index);

        if (tempSlotUI.InvenSlot.IsEmpty) 
        {
            tempSlotUI.Close();
        }
        detaiInfo.IsPause = false;
        if (isSlotEnd)                              //�������Ŀ� ������ ���̰�
        {
            detaiInfo.Open(inven[index].ItemData);
        }
    }

    private void OnSlotClick(uint index)
    {
        if (!tempSlotUI.InvenSlot.IsEmpty) 
        {
            OnItemMoveEnd(index, true);     //������ Ŭ�������� ����-> �׻�Ʈ��
        }
    }

    private void OnSlotPointerMove(Vector2 screen)
    {
        detaiInfo.MovePosition(screen);
    }

    private void OnItemDetailOff()
    {
        detaiInfo.Close();
    }

    private void OnItemDetailOn(uint index)
    {
        detaiInfo.Open(slotUIs[index].InvenSlot.ItemData);
    }
}
