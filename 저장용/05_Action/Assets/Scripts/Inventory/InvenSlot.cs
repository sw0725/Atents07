using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenSlot
{
    public uint Index => slotIndex;
    public Action onSlotItemChange;         //슬롯에 들간 아이템의 종류, 개수, 장비여부의 변경
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

    public void AssignSlotItem(ItemData data, uint count = 1, bool isEquip = false) //슬롯에 아이템 설정(줍기) 
    {
        if (data != null)
        {
            ItemData = data;
            ItemCount = count;
            IsEquipped = isEquip;
            Debug.Log($"인벤토리 {slotIndex}번 슬롯에 [{ItemData.itemName}]아이템 [{itemCount}]추가");
        }
        else 
        {
            ClearSlotItem();
        }
    }

    public void ClearSlotItem()       //슬롯에 아이템 제거(버리기)
    {
        ItemData = null;
        ItemCount = 0;
        IsEquipped = false;
        Debug.Log($"인벤토리 {slotIndex}번 슬롯을 비웁니다.");
    }

    public bool IncreaseSlotItem(out uint overCount, uint increaseCount = 1)    //디폴트 매개변수는 가장 나중에 적어준다(구분이 안가서)
    {       //bool인 이유 ItemData의 maxStackCount 때문. 오버(실패)했는가와 얼마나 넘치는지를 반환, 리턴은 하나뿐이기에 나머지는 out으로
        bool result;
        uint newCount = ItemCount + increaseCount;
        int over = (int)newCount - (int)ItemData.maxStackCount;

        Debug.Log($"인벤토리 [{slotIndex}]번 슬롯에 아이템 증가, 현재[{ItemCount}]개");
        if (over > 0)//넘침
        {
            ItemCount = ItemData.maxStackCount;
            overCount = (uint)over;
            result = false;
            Debug.Log($"아이템 최대치까지 증가, [{over}]개 넘침");
        }
        else 
        {
            ItemCount = newCount;
            overCount = 0;
            result = true;
            Debug.Log($"아이템 [{increaseCount}]개 증가.");
        }
        return result;
    }

    public void DeCreaseSlotItem(uint decreaseCount = 1) 
    {
        int newCount = (int)ItemCount - (int)decreaseCount;
        if (newCount > 0)   //남았다
        {
            ItemCount = (uint)newCount;
            Debug.Log($"인벤토리 [{slotIndex}]번 슬롯에 [{ItemData.itemName}]이 [{decreaseCount}]개 감소, 현재[{ItemCount}]개");
        }
        else 
        {
            ClearSlotItem();
        }
    }

    public void UseItem(GameObject target)            //아이템 사용
    {
        
    }

    public void EquipItem(GameObject target)          //아이템 장비
    {

    }
}
