using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempSlotUI : SlotUIBase      //TempSlot은 드래그시 아이템이 슬롯을 벗어나도 보이게 하기 위해 사용된다. 마우스를 슬롯화 하고 그 슬롯에 아이템을 넣어 이동하는 방식
{
    InvenTempSlot tempSlot;
    
    public uint FromIndex => tempSlot.FromIndex;

    private void Update()                                                            //집은 물건이 마우스를 따라다니게 하기 위함
    {
        transform.position = Mouse.current.position.ReadValue();                     //마우스 스크린 좌표(UI사용 좌표)
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
