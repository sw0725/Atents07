using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempSlotUI : SlotUIBase      //TempSlot은 드래그시 아이템이 슬롯을 벗어나도 보이게 하기 위해 사용된다. 마우스를 슬롯화 하고 그 슬롯에 아이템을 넣어 이동하는 방식
{
    InvenTempSlot tempSlot;
    Player owner;
    
    public uint FromIndex => tempSlot.FromIndex;

    private void Update()                                                            //집은 물건이 마우스를 따라다니게 하기 위함
    {
        transform.position = Mouse.current.position.ReadValue();                     //마우스 스크린 좌표(UI사용 좌표)
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
                Vector3 dropPosition = hitInfo.point;                                                       //벽에 부닥칠때 그 아래에 생성
                dropPosition.y = 0;

                Vector3 dropDir = dropPosition - owner.transform.position;
                if (dropDir.sqrMagnitude > owner.ItemPickUpRange * owner.ItemPickUpRange)                   //길이비교 정확한 길이 계산은 정말 안좋기때문에 sqrMagnitude를 사용
                {
                    dropPosition = dropDir.normalized * owner.ItemPickUpRange + owner.transform.position;
                }

                Factory.Instance.MakeItems(InvenSlot.ItemData.code, InvenSlot.ItemCount, dropPosition, (InvenSlot.ItemCount > 1));
                InvenSlot.ClearSlotItem();
            }
        }
    }
}
