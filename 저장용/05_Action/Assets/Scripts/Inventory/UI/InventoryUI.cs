using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Player Owner => inven.Owner;

    Inventory inven;            //보여줄 인벤토리
    PlayerInputAction inputAction;
    CanvasGroup canvasGroup;

    InvenSlotUI[] slotUIs;      //안의 슬롯 ui
    TempSlotUI tempSlotUI;
    DetaiInfo detaiInfo;
    ItemDividerUI itemDividerUI;
    MoneyPanelUI moneyPanelUI;
    SortPanelUI sortPanelUI;

    private void Awake()
    {
        Transform c = transform.GetChild(0);
        slotUIs = c.GetComponentsInChildren<InvenSlotUI>();

        c = transform.GetChild(3);
        Button close = c.GetComponent<Button>();
        close.onClick.AddListener(Close);

        tempSlotUI = GetComponentInChildren<TempSlotUI>();
        detaiInfo = GetComponentInChildren<DetaiInfo>();
        itemDividerUI = GetComponentInChildren<ItemDividerUI>(true);
        moneyPanelUI = GetComponentInChildren<MoneyPanelUI>();
        sortPanelUI = GetComponentInChildren<SortPanelUI>();

        inputAction = new PlayerInputAction();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        inputAction.UI.Enable();
        inputAction.UI.InvenOnOff.performed += onInvenOnOff;
        inputAction.UI.Click.canceled += OnItemDrop;
    }

    private void OnDisable()
    {
        inputAction.UI.Click.canceled -= OnItemDrop;
        inputAction.UI.InvenOnOff.performed -= onInvenOnOff;
        inputAction.UI.Disable();
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

        itemDividerUI.onOkClick += OnDividerOk;
        itemDividerUI.onCancle += OnDividerCancle;
        itemDividerUI.Close();

        Owner.onMoneyChange += moneyPanelUI.Refresh;
        moneyPanelUI.Refresh(Owner.Money);

        sortPanelUI.onSortRequest += ((by) => 
        {
            bool isAcending = true;
            switch (by) 
            {
                case ItemSortBy.Price:
                    isAcending = false;
                    break;
            }
            inven.MergeItems();
            inven.SlotSorting(by, isAcending);
        });

        Close();
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
        if (tempSlotUI.InvenSlot.IsEmpty)
        {
            bool isShiftPress = (Keyboard.current.shiftKey.ReadValue() > 0);
            if (isShiftPress) 
            {
                OnItemDividerOpen(index);
            }
            else    //아이템사용 or장비
            {
            
            }
        }
        else                                //임시슬롯에 뭐 있음
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
    void OnItemDividerOpen(uint index)      //분리창 열기
    {
        InvenSlotUI target = slotUIs[index];
        itemDividerUI.transform.position = target.transform.position + Vector3.up * 100;
        if (itemDividerUI.Open(target.InvenSlot)) 
        {
            detaiInfo.IsPause = true;
        }
    }

    private void OnDividerCancle()
    {
        detaiInfo.IsPause = false;
    }

    private void OnDividerOk(uint targetIndex, uint dividCount)
    {
        inven.DividItem(targetIndex, dividCount);
        tempSlotUI.Open();
    }

    void Open() 
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable= true;
        canvasGroup.blocksRaycasts = true;
    }

    void Close() 
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void onInvenOnOff(InputAction.CallbackContext context)
    {
        if (canvasGroup.interactable)
        {
            Close();
        }
        else 
        {
            Open();
        }
    }

    private void OnItemDrop(InputAction.CallbackContext _)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 diff = screenPos - (Vector2)transform.position;
        RectTransform rect = (RectTransform)transform;

        if (!rect.rect.Contains(diff)) //영역밖
        {
            tempSlotUI.OnDrop(screenPos);
        }
    }
}
