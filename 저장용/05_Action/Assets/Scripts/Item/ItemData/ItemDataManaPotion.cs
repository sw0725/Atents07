using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data ManaPotion", menuName = "Scriptable Object/Item Data ManaPotion", order = 5)]
public class ItemDataManaPotion : ItemData, IUsable
{
    [Header("���� ���� ����")]
    public float totalRegen = 50.0f;
    public float duration = 1.0f;

    public bool Use(GameObject target)
    {
        bool result = false;
        IMana mana = target.GetComponent<IMana>();
        if (mana != null)
        {
            if (mana.MP < mana.MaxMP)
            {
                mana.ManaRegernerate(totalRegen, duration);
                result = true;
            }
            else 
            {
                Debug.Log("���� �ִ�ġ");
            }
        }
        return result;
    }
}
