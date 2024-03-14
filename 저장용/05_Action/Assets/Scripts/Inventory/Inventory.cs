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
        if (!IsValidIndex(slotIndex))
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
        if (!IsValidIndex(slotIndex))
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

    bool IsValidIndex(uint index)                                           //��밡���� �ε������� Ȯ��
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
                inter[i] = "(��ĭ)";
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
