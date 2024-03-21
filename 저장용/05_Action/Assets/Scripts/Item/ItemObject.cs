using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : RecycleObject
{
    public ItemData ItemData 
    {
        get => itemData;
        set 
        {
            if (itemData == null)                           //Ȱ��ȭ ���Ŀ��� �� �ѹ��� ��������-���丮����
            {
                itemData = value;
                spriteRenderer.sprite = itemData.itemIcon;
            }
        }
    }
    ItemData itemData = null;
    SpriteRenderer spriteRenderer = null;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void OnEnable()
    {
        itemData = null;
        base.OnEnable();
    }

    public void End()
    {
        gameObject.SetActive(false);
    }
}
