using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvenSlotUI : SlotUIBase, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Action<uint> onDragBegin;               //uint 드래그가 시작된 슬롯
    public Action<uint, bool> onDragEnd;           //슬롯에서 드래그가 끝날시 true
    TextMeshProUGUI equipText;

    protected override void Awake()
    {
        base.Awake();
        Transform c = transform.GetChild(2);
        equipText = c.GetComponent<TextMeshProUGUI>();
    }

    protected override void OnRefresh()
    {
        if (InvenSlot.IsEquipped)
        {
            equipText.color = Color.red;
        }
        else
        {
            equipText.color = Color.clear;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"드래그 시작 : [{Index}]번 슬롯");
        onDragBegin?.Invoke(Index);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;    //ui대상 레이캐스트
        if (obj != null)
        {
            InvenSlotUI endSlot = obj.GetComponent<InvenSlotUI>();
            if (endSlot != null)
            {
                Debug.Log($"드래그 종료 : [{endSlot.Index}]번 슬롯");
                onDragEnd?.Invoke(endSlot.Index, true);
            }
            else 
            {
                Debug.Log("슬롯 아님");
                onDragEnd?.Invoke(Index, false);
            }
        }
        else 
        {
            Debug.Log("UI없음");
        }
    }
}
