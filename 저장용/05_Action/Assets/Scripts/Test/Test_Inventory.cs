using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Inventory : TestBase
{
    private void Start()
    {
        ItemData itemData = GameManager.Instance.ItemData[ItemCode.Ruby];
    }
}
