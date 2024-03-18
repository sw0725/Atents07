using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempSlotUI : SlotUIBase      //TempSlot�� �巡�׽� �������� ������ ����� ���̰� �ϱ� ���� ���ȴ�. ���콺�� ����ȭ �ϰ� �� ���Կ� �������� �־� �̵��ϴ� ���
{
    InvenTempSlot tempSlot;
    
    public uint FromIndex => tempSlot.FromIndex;

    private void Update()                                                            //���� ������ ���콺�� ����ٴϰ� �ϱ� ����
    {
        transform.position = Mouse.current.position.ReadValue();                     //���콺 ��ũ�� ��ǥ(UI��� ��ǥ)
    }

    public override void InitalizeSlot(InvenSlot slot)
    {
        base.InitalizeSlot(slot);
        tempSlot = slot as InvenTempSlot;
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
}
