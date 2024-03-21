using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item Data Coin", menuName = "Scriptable Object/Item Data Coin", order = 1)]
public class ItemDataCoin : ItemData, IConumable
{
    public void Consume(GameObject target)
    {
        Player player = target.GetComponent<Player>();
        if (player != null) 
        {
            //타겟은 플레이어
            player.Money += (int)price;
        }
    }
}
