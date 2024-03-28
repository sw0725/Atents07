using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenSlot
{
    public uint Index => slotIndex;
    public Action onSlotItemChange;         //���Կ� �鰣 �������� ����, ����, ��񿩺��� ����
    public Action<InvenSlot> onItemEquip;
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
            if (isEquipped) 
            {
                onItemEquip?.Invoke(this);
            }
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

    public void AssignSlotItem(ItemData data, uint count = 1, bool isEquip = false)   //���Կ� ������ ����(�ݱ�) 
    {                                                                                             
        if (data != null)
        {
            ItemData = data;
            ItemCount = count;
            IsEquipped = isEquip;
        }
        else 
        {
            ClearSlotItem();
        }
    }

    public virtual void ClearSlotItem()       //���Կ� ������ ����(������)
    {
        ItemData = null;
        ItemCount = 0;
        IsEquipped = false;
    }

    public bool IncreaseSlotItem(out uint overCount, uint increaseCount = 1)    //����Ʈ �Ű������� ���� ���߿� �����ش�(������ �Ȱ���)
    {       //bool�� ���� ItemData�� maxStackCount ����. ����(����)�ߴ°��� �󸶳� ��ġ������ ��ȯ, ������ �ϳ����̱⿡ �������� out����
        bool result;
        uint newCount = ItemCount + increaseCount;
        int over = (int)newCount - (int)ItemData.maxStackCount;

        if (over > 0)//��ħ
        {
            ItemCount = ItemData.maxStackCount;
            overCount = (uint)over;
            result = false;
        }
        else 
        {
            ItemCount = newCount;
            overCount = 0;
            result = true;
        }
        return result;
    }

    public void DeCreaseSlotItem(uint decreaseCount = 1) 
    {
        int newCount = (int)ItemCount - (int)decreaseCount;
        if (newCount > 0)   //���Ҵ�
        {
            ItemCount = (uint)newCount;
        }
        else 
        {
            ClearSlotItem();
        }
    }

    public void UseItem(GameObject target)            //������ ���
    {
        IUsable usable = ItemData as IUsable;         //ItemData�� ScriptableObject�� MonoBehaviour�� �ƴϹǷ� ��� ���� �����ϴ� ��������Ʈ(����Ƽ ����)�� ���Ұ� 
        if (usable != null) 
        {
            if (usable.Use(target)) 
            {
                DeCreaseSlotItem();
            }
        }
    }

    public void EquipItem(GameObject target)          //������ ���
    {
        IEquipable equipable = ItemData as IEquipable;
        if (equipable != null) 
        {
            equipable.ToggleEquip(target, this);
        }
    }
}
