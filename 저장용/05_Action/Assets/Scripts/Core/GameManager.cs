using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singltrun<GameManager>
{
    Player player;

    public Player Player => player;

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
    }

    ItemDataManager itemDataManager;
    public ItemDataManager ItemData => itemDataManager;

    protected override void OnpreInitialize()
    {
        base.OnpreInitialize();
        itemDataManager = GetComponent<ItemDataManager>();
    }

}
