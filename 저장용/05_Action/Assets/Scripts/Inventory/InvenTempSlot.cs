using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenTempSlot : InvenSlot
{
    const uint NotSet = uint.MaxValue;

    uint fromIndex = NotSet;
    public uint FromIndex => fromIndex;
    
    public InvenTempSlot(uint index) : base(index)           //부모 생성자도 같이실행
    {
        fromIndex = NotSet;
    }
                                                            //a ?? b  =>  ?? 연산자 a가 널이 아니면 a, 널이면 b 
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
