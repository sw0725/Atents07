using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenSlot
{
    public uint Index => slotIndex;
    public Action onSlotItemChange;         //���Կ� �鰣 �������� ����, ����, ��񿩺��� ����
    public ItemData ItemData                //���Կ� �鰣 ������ ���� Ȯ��
    {
        get => slotItemData;
        private set
        {
            if (slotItemData != value)
            {
                slotItemData = value;
                onSlotItemChange?.Invoke();
            }
        }
    }
    public uint ItemCount 
    {
        get => itemCount;
        set 
        {
            if (itemCount != value) 
            {
                itemCount = value;
                onSlotItemChange?.Invoke();
            }
        }
    }
    public bool IsEquipped 
    {
        get => isEquipped;
        set 
        {
            isEquipped = value;
            onSlotItemChange?.Invoke();
        }
    }
    public bool IsEmpty => slotItemData == null;

    uint slotIndex;                         //�κ��丮���� ���° �����ΰ�(������ȣ)
    ItemData slotItemData = null;           //���Կ� �鰣 ������ ����
    uint itemCount = 0;
    bool isEquipped = false;

    public InvenSlot(uint index)            //������ 
    {
        slotIndex = index;                  //�����ÿ��� ����, ���� ������ �ʾƾ���
        ItemData = null;
        ItemCount = 0;
        IsEquipped = false;
    }

    public void AssignSlotItem(ItemData data, uint count = 1, bool isEquip = false) //���Կ� ������ ����(�ݱ�) 
    {
        if (data != null)
        {
            ItemData = data;
            ItemCount = count;
            IsEquipped = isEquip;
            Debug.Log($"�κ��丮 {slotIndex}�� ���Կ� [{ItemData.itemName}]������ [{itemCount}]�߰�");
        }
        else 
        {
            ClearSlotItem();
        }
    }

    public void ClearSlotItem()       //���Կ� ������ ����(������)
    {
        ItemData = null;
        ItemCount = 0;
        IsEquipped = false;
        Debug.Log($"�κ��丮 {slotIndex}�� ������ ���ϴ�.");
    }

    public bool IncreaseSlotItem(out uint overCount, uint increaseCount = 1)    //����Ʈ �Ű������� ���� ���߿� �����ش�(������ �Ȱ���)
    {       //bool�� ���� ItemData�� maxStackCount ����. ����(����)�ߴ°��� �󸶳� ��ġ������ ��ȯ, ������ �ϳ����̱⿡ �������� out����
        bool result;
        uint newCount = ItemCount + increaseCount;
        int over = (int)newCount - (int)ItemData.maxStackCount;

        Debug.Log($"�κ��丮 [{slotIndex}]�� ���Կ� ������ ����, ����[{ItemCount}]��");
        if (over > 0)//��ħ
        {
            ItemCount = ItemData.maxStackCount;
            overCount = (uint)over;
            result = false;
            Debug.Log($"������ �ִ�ġ���� ����, [{over}]�� ��ħ");
        }
        else 
        {
            ItemCount = newCount;
            overCount = 0;
            result = true;
            Debug.Log($"������ [{increaseCount}]�� ����.");
        }
        return result;
    }

    public void DeCreaseSlotItem(uint decreaseCount = 1) 
    {
        int newCount = (int)ItemCount - (int)decreaseCount;
        if (newCount > 0)   //���Ҵ�
        {
            ItemCount = (uint)newCount;
            Debug.Log($"�κ��丮 [{slotIndex}]�� ���Կ� [{ItemData.itemName}]�� [{decreaseCount}]�� ����, ����[{ItemCount}]��");
        }
        else 
        {
            ClearSlotItem();
        }
    }

    public void UseItem(GameObject target)            //������ ���
    {
        
    }

    public void EquipItem(GameObject target)          //������ ���
    {

    }
}
