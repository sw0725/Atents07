using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Player Owner => inven.Owner;

    Inventory inven;            //������ �κ��丮
    PlayerInputAction inputAction;
    CanvasGroup canvasGroup;

    InvenSlotUI[] slotUIs;      //���� ���� ui
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
        if (tempSlotUI.InvenSlot.IsEmpty)
        {
            bool isShiftPress = (Keyboard.current.shiftKey.ReadValue() > 0);
            if (isShiftPress) 
            {
                OnItemDividerOpen(index);
            }
            else    //�����ۻ�� or���
            {
            
            }
        }
        else                                //�ӽý��Կ� �� ����
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
    void OnItemDividerOpen(uint index)      //�и�â ����
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

        if (!rect.rect.Contains(diff)) //������
        {
            tempSlotUI.OnDrop(screenPos);
        }
    }
}
