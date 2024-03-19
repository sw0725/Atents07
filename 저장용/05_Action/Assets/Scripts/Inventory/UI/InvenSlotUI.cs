using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvenSlotUI : SlotUIBase, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public Action<uint> onDragBegin;               //uint 드래그가 시작된 슬롯
    public Action<uint, bool> onDragEnd;           //슬롯에서 드래그가 끝날시 true
    public Action<uint> onClick;

    public Action<uint> onPointerEnter;
    public Action onPointerExit;
    public Action<Vector2> onPointerMove;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke(Index);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke(Index);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit?.Invoke();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        onPointerMove?.Invoke(eventData.position);  //마우스 스크린좌표 넘기기
    }
}
