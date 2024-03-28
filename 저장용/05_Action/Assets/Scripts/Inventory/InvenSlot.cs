using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenSlot
{
    public uint Index => slotIndex;
    public Action onSlotItemChange;         //슬롯에 들간 아이템의 종류, 개수, 장비여부의 변경
    public Action<InvenSlot> onItemEquip;
    public ItemData ItemData                //슬롯에 들간 아이템 종류 확인
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

    uint slotIndex;                         //인벤토리에서 몇번째 슬롯인가(고유번호)
    ItemData slotItemData = null;           //슬롯에 들간 아이템 종류
    uint itemCount = 0;
    bool isEquipped = false;

    public InvenSlot(uint index)            //생성자 
    {
        slotIndex = index;                  //생성시에만 설정, 이후 변하지 않아야함
        ItemData = null;
        ItemCount = 0;
        IsEquipped = false;
    }

    public void AssignSlotItem(ItemData data, uint count = 1, bool isEquip = false)   //슬롯에 아이템 설정(줍기) 
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

    public virtual void ClearSlotItem()       //슬롯에 아이템 제거(버리기)
    {
        ItemData = null;
        ItemCount = 0;
        IsEquipped = false;
    }

    public bool IncreaseSlotItem(out uint overCount, uint increaseCount = 1)    //디폴트 매개변수는 가장 나중에 적어준다(구분이 안가서)
    {       //bool인 이유 ItemData의 maxStackCount 때문. 오버(실패)했는가와 얼마나 넘치는지를 반환, 리턴은 하나뿐이기에 나머지는 out으로
        bool result;
        uint newCount = ItemCount + increaseCount;
        int over = (int)newCount - (int)ItemData.maxStackCount;

        if (over > 0)//넘침
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
        if (newCount > 0)   //남았다
        {
            ItemCount = (uint)newCount;
        }
        else 
        {
            ClearSlotItem();
        }
    }

    public void UseItem(GameObject target)            //아이템 사용
    {
        IUsable usable = ItemData as IUsable;         //ItemData는 ScriptableObject로 MonoBehaviour가 아니므로 모노 에서 지원하는 겟컴포넌트(유니티 지원)는 사용불가 
        if (usable != null) 
        {
            if (usable.Use(target)) 
            {
                DeCreaseSlotItem();
            }
        }
    }

    public void EquipItem(GameObject target)          //아이템 장비
    {
        IEquipable equipable = ItemData as IEquipable;
        if (equipable != null) 
        {
            equipable.ToggleEquip(target, this);
        }
    }
}
