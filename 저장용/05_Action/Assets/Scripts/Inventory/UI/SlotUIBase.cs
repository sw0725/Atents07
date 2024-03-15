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

    public virtual void InitalizeSlot(InvenSlot slot)       //���� �ʱ�ȭ �ϱ� => �κ����԰� ui����
    {
        invenSlot = slot;
        invenSlot.onSlotItemChange = Refresh;               //���� ���� ����
        Refresh();
    }

    private void Refresh()
    {
        if (InvenSlot.IsEmpty)
        {
            itemIcon.color = Color.clear;                   //������ ����
            itemIcon.sprite = null;
            itemCount.text = String.Empty;                  //����, ��������Ʈ ����
        }
        else 
        {
            itemIcon.sprite = InvenSlot.ItemData.itemIcon;  //����, ��������Ʈ ����
            itemIcon.color = Color.white;                   //������ ����ȭ
            itemCount.text = InvenSlot.ItemCount.ToString();
        }
        OnRefresh();
    }

    protected virtual void OnRefresh()
    {
        //��񿩺� ǥ��
    }
}
