using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;            //보여줄 인벤토리

    InvenSlotUI[] slotUIs;      //안의 슬롯 ui
    TempSlotUI tempSlotUI;
    DetaiInfo detaiInfo;

    private void Awake()
    {
        Transform c = transform.GetChild(0);
        slotUIs = c.GetComponentsInChildren<InvenSlotUI>();

        tempSlotUI = GetComponentInChildren<TempSlotUI>();
        detaiInfo = GetComponentInChildren<DetaiInfo>();
    }
    public void InitializeInventory(Inventory playerInventory)  //초기화
    {
        inven = playerInventory;

        for (uint i = 0; i < slotUIs.Length; i++) 
        {
            slotUIs[i].InitalizeSlot(inven[i]);                 //안의 슬롯 초기화(연결)
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
        detaiInfo.IsPause = true;               //드래그할때 안보이게
        inven.MoveItem(index, tempSlotUI.Index);
        tempSlotUI.Open();
    }

    private void OnItemMoveEnd(uint index, bool isSlotEnd)
    {                               //슬롯에서 드래그 끝나면 끝슬롯 아니면 시작슬롯
        //uint finalIndex = index;
        //if (isSlotEnd) 
        //{
        //    if (inven.FindEmptySlot(out uint emptySlotIndex))
        //    {
        //        finalIndex = emptySlotIndex;
        //    }
        //    else 
        //    {
        //        //바닥에 드랍
        //        Debug.Log("아이템 드롭 필요");
        //        return;
        //    }
        //}
        inven.MoveItem(tempSlotUI.Index, index);

        if (tempSlotUI.InvenSlot.IsEmpty) 
        {
            tempSlotUI.Close();
        }
        detaiInfo.IsPause = false;
        if (isSlotEnd)                              //놓은직후에 아이템 보이게
        {
            detaiInfo.Open(inven[index].ItemData);
        }
    }

    private void OnSlotClick(uint index)
    {
        if (!tempSlotUI.InvenSlot.IsEmpty) 
        {
            OnItemMoveEnd(index, true);     //슬롯이 클릭됫을때 실행-> 항상트루
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
