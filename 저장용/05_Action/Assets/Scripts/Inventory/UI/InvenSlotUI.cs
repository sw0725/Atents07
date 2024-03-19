using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvenSlotUI : SlotUIBase, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public Action<uint> onDragBegin;               //uint �巡�װ� ���۵� ����
    public Action<uint, bool> onDragEnd;           //���Կ��� �巡�װ� ������ true
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
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;    //ui��� ����ĳ��Ʈ
        if (obj != null)
        {
            InvenSlotUI endSlot = obj.GetComponent<InvenSlotUI>();
            if (endSlot != null)
            {
                onDragEnd?.Invoke(endSlot.Index, true);
            }
            else 
            {
                Debug.Log("���� �ƴ�");
                onDragEnd?.Invoke(Index, false);
            }
        }
        else 
        {
            Debug.Log("UI����");
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
        onPointerMove?.Invoke(eventData.position);  //���콺 ��ũ����ǥ �ѱ��
    }
}
