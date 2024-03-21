using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempSlotUI : SlotUIBase      //TempSlot�� �巡�׽� �������� ������ ����� ���̰� �ϱ� ���� ���ȴ�. ���콺�� ����ȭ �ϰ� �� ���Կ� �������� �־� �̵��ϴ� ���
{
    InvenTempSlot tempSlot;
    Player owner;
    
    public uint FromIndex => tempSlot.FromIndex;

    private void Update()                                                            //���� ������ ���콺�� ����ٴϰ� �ϱ� ����
    {
        transform.position = Mouse.current.position.ReadValue();                     //���콺 ��ũ�� ��ǥ(UI��� ��ǥ)
    }

    public override void InitalizeSlot(InvenSlot slot)
    {
        base.InitalizeSlot(slot);
        tempSlot = slot as InvenTempSlot;
        owner = GameManager.Instance.InventoryUI.Owner;
        Close();
    }

    public void Open() 
    {
        transform.position = Mouse.current.position.ReadValue();
        gameObject.SetActive(true);
    }

    public void Close() 
    {
        gameObject.SetActive(false);
    }

    public void SetFromIndex(uint index) 
    {
        tempSlot.SetFromIndex(index);
    }

    public void OnDrop(Vector2 screenPos)
    {
        if (!InvenSlot.IsEmpty) 
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000.0f, LayerMask.GetMask("Ground"))) 
            {
                Vector3 dropPosition = hitInfo.point;                                                       //���� �δ�ĥ�� �� �Ʒ��� ����
                dropPosition.y = 0;

                Vector3 dropDir = dropPosition - owner.transform.position;
                if (dropDir.sqrMagnitude > owner.ItemPickUpRange * owner.ItemPickUpRange)                   //���̺� ��Ȯ�� ���� ����� ���� �����⶧���� sqrMagnitude�� ���
                {
                    dropPosition = dropDir.normalized * owner.ItemPickUpRange + owner.transform.position;
                }

                Factory.Instance.MakeItems(InvenSlot.ItemData.code, InvenSlot.ItemCount, dropPosition, (InvenSlot.ItemCount > 1));
                InvenSlot.ClearSlotItem();
            }
        }
    }
}
