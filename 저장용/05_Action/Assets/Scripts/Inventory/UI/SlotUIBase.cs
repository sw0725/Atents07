using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUIBase : MonoBehaviour
{
    public InvenSlot InvenSlot => invenSlot;
    public uint Index => InvenSlot.Index;
    InvenSlot invenSlot;

    Image itemIcon;
    TextMeshProUGUI itemCount;

    protected virtual void Awake()
    {
        Transform c = transform.GetChild(0);
        itemIcon = c.GetComponent<Image>();
        c = transform.GetChild(1);
        itemCount = c.GetComponent<TextMeshProUGUI>();
    }

    public virtual void InitalizeSlot(InvenSlot slot)       //슬롯 초기화 하기 => 인벤슬롯과 ui연결
    {
        invenSlot = slot;
        invenSlot.onSlotItemChange = Refresh;               //이전 연결 제거
        Refresh();
    }

    private void Refresh()
    {
        if (InvenSlot.IsEmpty)
        {
            itemIcon.color = Color.clear;                   //아이콘 투명
            itemIcon.sprite = null;
            itemCount.text = String.Empty;                  //글자, 스프라이트 제거
        }
        else 
        {
            itemIcon.sprite = InvenSlot.ItemData.itemIcon;  //글자, 스프라이트 세팅
            itemIcon.color = Color.white;                   //아이콘 가시화
            itemCount.text = InvenSlot.ItemCount.ToString();
        }
        OnRefresh();
    }

    protected virtual void OnRefresh()
    {
        //장비여부 표시
    }
}
