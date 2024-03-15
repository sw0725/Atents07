using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    const int Default_Inventory_Size = 6;               //�κ��丮 �⺻ ���� ���� - 6ĭ

    InvenSlot[] slots;
    int SlotCount => slots.Length;
    public InvenSlot this[uint index] => slots[index];  //�κ��丮 ���Կ� ���� ���� �ε���

    InvenSlot tempSlot;                                 //�ӽý���(������ ��ġ �ٲܶ� ����)
    uint tempSlotIndex = 1111111;
    public InvenSlot TempSlot => tempSlot;

    ItemDataManager ItemDataManager;                    //������ ������ �Ŵ��� - ������ ������ ���� ��� ����
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
        ItemDataManager = GameManager.Instance.ItemData;    //�̰Ŷ����� �κ��丮�� ��ŸƮ�������� ���������ϴ�

        this.owner = owner;
    }

    public bool AddItem(ItemCode code)                      //�κ��丮�� ������ 1�� �߰�
    {       //�κ��丮 ĭ�� �������ų�, ������ �������� �ƽ��� ������ ��� ���и� �˸��� ����
        for (int i = 0; i < SlotCount; i++) 
        {
            if(AddItem(code, (uint)i))
                return true;
        }
        return false;
    }

    public bool AddItem(ItemCode code, uint slotIndex)      //Ư�� ���Կ� ������ 1�� �߰�
    {
        bool result = false;
        if (IsValidIndex(slotIndex))
        {
            ItemData data = ItemDataManager[code];          //������ ������
            InvenSlot slot = slots[slotIndex];              //����
            if (slot.IsEmpty)                               //�����
            {
                slot.AssignSlotItem(data);
                result = true;
            }
            else 
            {
                if (slot.ItemData == data)                  //��������?
                {
                    result = slot.IncreaseSlotItem(out _); //�ϳ��ִ°Ŷ� ��ġ�� ������ �ǹ� ����.
                }
                else 
                {
                    Debug.Log($"������ �߰� ���� : [{slotIndex}]�� �ٸ� �������� ����ֽ��ϴ�.");
                }
            }
        }
        else 
        {
            Debug.Log($"������ �߰� ���� : [{slotIndex}]�� �߸��� �ε����Դϴ�.");
        }
        return result;
    }

    public void RemoveItem(uint slotIndex, uint decreaseCount = 1)          //Ư�� ������ ������ n�� ����
    {
        if (IsValidIndex(slotIndex))
        {
            InvenSlot slot = slots[slotIndex];
            slot.DeCreaseSlotItem(decreaseCount);
        }
        else 
        {
            Debug.Log($"������ ���� ���� : [{slotIndex}]�� �߸��� �ε����Դϴ�.");
        }
    }

    public void ClearSlot(uint slotIndex)                                   //Ư�� ���� ������ ��� ����
    {
        if (IsValidIndex(slotIndex))
        {
            InvenSlot slot = slots[slotIndex];
            slot.ClearSlotItem();
        }
        else
        {
            Debug.Log($"������ ���� ���� : [{slotIndex}]�� �߸��� �ε����Դϴ�.");
        }
    }

    public void ClearInventory()                                            //�κ��ʱ�ȭ
    {
        foreach (var slot in slots) 
        {
            slot.ClearSlotItem();
        }
    }

    public void MoveItem(uint from, uint to)                                //�κ��丮�� �������� from -> to �ű�
    {
        if ((from != to) && IsValidIndex(from) && IsValidIndex(to)) 
        {
            InvenSlot fromSlot = (from == tempSlotIndex) ? TempSlot : slots[from];

            if (!fromSlot.IsEmpty) 
            {
                InvenSlot toSlot = (to == tempSlotIndex) ? TempSlot : slots[to];

                if (fromSlot.ItemData == toSlot.ItemData)                   //������ ������
                {                                                           //to�� �ִ�ġ�� form�� ������ ����
                    toSlot.IncreaseSlotItem(out uint overcount, fromSlot.ItemCount);
                    fromSlot.DeCreaseSlotItem(fromSlot.ItemCount - overcount);      //over�� from�� ���� �����̹Ƿ�
                    Debug.Log($"[{from}]�� ���Կ��� [{to}]�� �������� ������ ��ġ��");
                }
                else                                                        //������ �������̳� ����ְų�
                {
                    ItemData tempData = fromSlot.ItemData;
                    uint tempCount = fromSlot.ItemCount;
                    bool tempEquip = fromSlot.IsEquipped;

                    fromSlot.AssignSlotItem(toSlot.ItemData, toSlot.ItemCount, toSlot.IsEquipped);
                    toSlot.AssignSlotItem(tempData, tempCount, tempEquip);
                    Debug.Log($"[{from}]�� ���Կ��� [{to}]�� ������ ������ ��ü");
                }
            }
        }
    }

    public void SplitItem(uint slotIndex, uint count)                       //Ư�����Կ��� �������� ������ ���� �ӽ� �������� ����
    {
        if (IsValidIndex(slotIndex)) 
        {
            InvenSlot slot = slots[slotIndex];
            count = System.Math.Min(count, slot.ItemCount);                 //count�� �鰣 �������� ũ�� ���Կ� �鰡�ִ� ���������� ���

            TempSlot.AssignSlotItem(slot.ItemData, count);
            slot.DeCreaseSlotItem(count);
        }
    }

    public void SlotSorting(ItemSortBy sortBy, bool isAcending = true)      //������ ����
    {                             //���ı���           //��� = ��������
        List<InvenSlot> temp = new List<InvenSlot>(slots);                  //����Ʈ ���鶧 ��ȣ�ȿ� �迭 ������ �̰ɷ� ����Ʈ �������(���� ����)
        
        switch(sortBy) 
        {
            case ItemSortBy.Code:                                           //���Ĺ���� ���� �ӽø���Ʈ ����
                temp.Sort((x, y) =>                                         //x, y �� temp ����Ʈ�� ����� 2��
                {
                    if (x.ItemData == null)                                 //x == null(���) => x�� �ڷ�
                    {
                        return 1;
                    }
                    if (y.ItemData == null)                                 //y == null(���) => x�� ������
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
        //�ӽø���Ʈ�� ������ ���Կ� ����
        List<(ItemData, uint, bool)> sortedData = new List<(ItemData, uint, bool)>(SlotCount); //Ʃ�� -> �ʿ� �����͸� �����ؼ� ��������
        foreach (var slot in temp) 
        {
            sortedData.Add((slot.ItemData, slot.ItemCount, slot.IsEquipped));
        }

        int index = 0;
        foreach (var data in sortedData)                                    //������ ���Կ� ����
        {
            slots[index].AssignSlotItem(data.Item1, data.Item2, data.Item3);
            index++;
        }
    }

    bool IsValidIndex(uint index)                                           //��밡���� �ε������� Ȯ��
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
                printText += "(��ĭ)";
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
            printText += "(��ĭ)";
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
