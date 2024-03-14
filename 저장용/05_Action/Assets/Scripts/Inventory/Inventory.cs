using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    const int Default_Inventory_Size = 6;               //인벤토리 기본 슬롯 개수 - 6칸

    InvenSlot[] slots;
    int SlotCount => slots.Length;
    public InvenSlot this[uint index] => slots[index];  //인벤토리 슬롯에 접근 위한 인덱서

    InvenSlot tempSlot;                                 //임시슬롯(아이템 위치 바꿀때 같은)
    uint tempSlotIndex = 1111111;
    public InvenSlot TempSlot => tempSlot;

    ItemDataManager ItemDataManager;                    //아이템 데이터 매니져 - 아이템 종류별 정보 모두 가짐
    Player owner;
    public Player Owner => owner;

    public Inventory(Player owner, uint size = Default_Inventory_Size) 
    {
        slots = new InvenSlot[size];
        for (uint i = 0; i < size; i++) 
        {
            slots[i] = new InvenSlot(i);
        }
        tempSlot = new InvenSlot(tempSlotIndex);
        ItemDataManager = GameManager.Instance.ItemData;    //이거때문에 인벤토리는 스타트에서나야 생성가능하다

        this.owner = owner;
    }

    public bool AddItem(ItemCode code)                      //인벤토리에 아이템 1개 추가
    {       //인벤토리 칸이 가득차거나, 동종의 아이템이 맥스에 도달한 경우 실패를 알리기 위함
        for (int i = 0; i < SlotCount; i++) 
        {
            if(AddItem(code, (uint)i))
                return true;
        }
        return false;
    }

    public bool AddItem(ItemCode code, uint slotIndex)      //특정 슬롯에 아이템 1개 추가
    {
        bool result = false;
        if (IsValidIndex(slotIndex))
        {
            ItemData data = ItemDataManager[code];          //데이터 가져옴
            InvenSlot slot = slots[slotIndex];              //슬롯
            if (slot.IsEmpty)                               //비었음
            {
                slot.AssignSlotItem(data);
                result = true;
            }
            else 
            {
                if (slot.ItemData == data)                  //같은종류?
                {
                    result = slot.IncreaseSlotItem(out _); //하나넣는거라 넘치는 갯수가 의미 없다.
                }
                else 
                {
                    Debug.Log($"아이템 추가 실패 : [{slotIndex}]에 다른 아이템이 들어있습니다.");
                }
            }
        }
        else 
        {
            Debug.Log($"아이템 추가 실패 : [{slotIndex}]는 잘못된 인덱스입니다.");
        }
        return result;
    }

    public void RemoveItem(uint slotIndex, uint decreaseCount = 1)          //특정 슬롯의 아이템 n개 제거
    {
        if (!IsValidIndex(slotIndex))
        {
            InvenSlot slot = slots[slotIndex];
            slot.DeCreaseSlotItem(decreaseCount);
        }
        else 
        {
            Debug.Log($"아이템 감소 실패 : [{slotIndex}]는 잘못된 인덱스입니다.");
        }
    }

    public void ClearSlot(uint slotIndex)                                   //특정 슬롯 아이템 모두 제거
    {
        if (!IsValidIndex(slotIndex))
        {
            InvenSlot slot = slots[slotIndex];
            slot.ClearSlotItem();
        }
        else
        {
            Debug.Log($"아이템 삭제 실패 : [{slotIndex}]는 잘못된 인덱스입니다.");
        }
    }

    public void ClearInventory()                                            //인벤초기화
    {
        foreach (var slot in slots) 
        {
            slot.ClearSlotItem();
        }
    }

    public void MoveItem(uint from, uint to)                                //인벤토리의 아이템을 from -> to 옮김
    {
        if ((from != to) && IsValidIndex(from) && IsValidIndex(to)) 
        {
            InvenSlot fromSlot = (from == tempSlotIndex) ? TempSlot : slots[from];

            if (!fromSlot.IsEmpty) 
            {
                InvenSlot toSlot = (to == tempSlotIndex) ? TempSlot : slots[to];

                if (fromSlot.ItemData == toSlot.ItemData)                   //동종의 아이템
                {                                                           //to를 최대치로 form은 나머지 갯수
                    toSlot.IncreaseSlotItem(out uint overcount, fromSlot.ItemCount);
                    fromSlot.DeCreaseSlotItem(fromSlot.ItemCount - overcount);      //over가 from이 가질 갯수이므로
                    Debug.Log($"[{from}]번 슬롯에서 [{to}]번 슬롯으로 아이템 합치기");
                }
                else                                                        //이종의 아이템이나 비어있거나
                {
                    ItemData tempData = fromSlot.ItemData;
                    uint tempCount = fromSlot.ItemCount;
                    bool tempEquip = fromSlot.IsEquipped;

                    fromSlot.AssignSlotItem(toSlot.ItemData, toSlot.ItemCount, toSlot.IsEquipped);
                    toSlot.AssignSlotItem(tempData, tempCount, tempEquip);
                    Debug.Log($"[{from}]번 슬롯에서 [{to}]번 슬롯의 아이템 교체");
                }
            }
        }
    }

    bool IsValidIndex(uint index)                                           //사용가능한 인덱스인지 확인
    {
        return (index < SlotCount) || (index == tempSlotIndex);
    }

#if UNITY_EDITOR
    public void Test_InventoryPrint() 
    {
        string[] inter = new string[6];
        for(int i = 0; i < SlotCount; i++)
        {
            if (slots[i].IsEmpty)
            {
                inter[i] = "(빈칸)";
            }
            else 
            {
                inter[i] = $"{slots[i].ItemData.itemName}({slots[i].ItemCount}/{slots[i].ItemData.maxStackCount})";
            }
        }
        Debug.Log($"{inter[0]}, {inter[1]}, {inter[2]}, {inter[3]}, {inter[4]}, {inter[5]}");
    }
#endif
}
