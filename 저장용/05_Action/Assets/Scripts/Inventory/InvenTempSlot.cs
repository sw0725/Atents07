using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenTempSlot : InvenSlot
{
    const uint NotSet = uint.MaxValue;

    uint fromIndex = NotSet;
    public uint FromIndex => fromIndex;
    
    public InvenTempSlot(uint index) : base(index)           //�θ� �����ڵ� ���̽���
    {
        fromIndex = NotSet;
    }
                                                            //a ?? b  =>  ?? ������ a�� ���� �ƴϸ� a, ���̸� b 
    public override void ClearSlotItem()
    {
        base.ClearSlotItem();
        fromIndex = NotSet;
    }

    public void SetFromIndex(uint index) 
    {
        fromIndex = index;
    }
}
