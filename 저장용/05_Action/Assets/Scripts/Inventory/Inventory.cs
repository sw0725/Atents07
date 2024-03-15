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
        if (IsValidIndex(slotIndex))
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
        if (IsValidIndex(slotIndex))
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

    public void SplitItem(uint slotIndex, uint count)                       //특정슬롯에서 아이템을 일정량 덜어 임시 슬롯으로 보냄
    {
        if (IsValidIndex(slotIndex)) 
        {
            InvenSlot slot = slots[slotIndex];
            count = System.Math.Min(count, slot.ItemCount);                 //count가 들간 개수보다 크면 슬롯에 들가있는 개수까지만 사용

            TempSlot.AssignSlotItem(slot.ItemData, count);
            slot.DeCreaseSlotItem(count);
        }
    }

    public void SlotSorting(ItemSortBy sortBy, bool isAcending = true)      //아이템 정렬
    {                             //정렬기준           //어센딩 = 오름차순
        List<InvenSlot> temp = new List<InvenSlot>(slots);                  //리스트 만들때 괄호안에 배열 넣으면 이걸로 리스트 만들어줌(정렬 이전)
        
        switch(sortBy) 
        {
            case ItemSortBy.Code:                                           //정렬방법에 따라 임시리스트 정렬
                temp.Sort((x, y) =>                                         //x, y 는 temp 리스트의 요소중 2개
                {
                    if (x.ItemData == null)                                 //x == null(빈거) => x를 뒤로
                    {
                        return 1;
                    }
                    if (y.ItemData == null)                                 //y == null(빈거) => x를 앞으로
                    {
                        return -1;
                    }

                    if (isAcending)
                    {
                        return x.ItemData.code.CompareTo(y.ItemData.code);
                    }
                    else 
                    {
                        return y.ItemData.code.CompareTo(x.ItemData.code);
                    }
                });
                break;
            case ItemSortBy.Name:
                temp.Sort((x, y) =>
                {
                    if (x.ItemData == null)
                    {
                        return 1;
                    }
                    if (y.ItemData == null)
                    {
                        return -1;
                    }

                    if (isAcending)
                    {
                        return x.ItemData.itemName.CompareTo(y.ItemData.itemName);
                    }
                    else
                    {
                        return y.ItemData.itemName.CompareTo(x.ItemData.itemName);
                    }
                });
                break;
            case ItemSortBy.Price:
                temp.Sort((x, y) =>
                {
                    if (x.ItemData == null)
                    {
                        return 1;
                    }
                    if (y.ItemData == null)
                    {
                        return -1;
                    }

                    if (isAcending)
                    {
                        return x.ItemData.price.CompareTo(y.ItemData.price);
                    }
                    else
                    {
                        return y.ItemData.price.CompareTo(x.ItemData.price);
                    }
                });
                break;

        }
        //임시리스트의 내용을 슬롯에 설정
        List<(ItemData, uint, bool)> sortedData = new List<(ItemData, uint, bool)>(SlotCount); //튜플 -> 필요 데이터만 복사해서 가져오기
        foreach (var slot in temp) 
        {
            sortedData.Add((slot.ItemData, slot.ItemCount, slot.IsEquipped));
        }

        int index = 0;
        foreach (var data in sortedData)                                    //내용을 슬롯에 설정
        {
            slots[index].AssignSlotItem(data.Item1, data.Item2, data.Item3);
            index++;
        }
    }

    bool IsValidIndex(uint index)                                           //사용가능한 인덱스인지 확인
    {
        return (index < SlotCount) || (index == tempSlotIndex);
    }

#if UNITY_EDITOR
    public void Test_InventoryPrint() 
    {
        string printText = "[";
        for (int i = 0; i < SlotCount-1; i++) 
        {
            if (slots[i].IsEmpty)
            {
                printText += "(빈칸)";
            }
            else 
            {
                printText += $"{slots[i].ItemData.itemName}({slots[i].ItemCount}/{slots[i].ItemData.maxStackCount})";
            }
            printText += ", ";
        }
        InvenSlot last = slots[SlotCount-1];
        if (last.IsEmpty)
        {
            printText += "(빈칸)";
        }
        else 
        {
            printText += $"{last.ItemData.itemName}({last.ItemCount}/{last.ItemData.maxStackCount})";
        }
        printText += "]";

        Debug.Log(printText);
    }
#endif
}
