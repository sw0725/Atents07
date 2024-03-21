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
            if (itemData == null)                           //활성화 이후에는 단 한번만 설정가능-팩토리에서
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
