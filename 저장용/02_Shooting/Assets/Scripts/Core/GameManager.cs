using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singltrun<GameManager>
{
    Player player;

    public Player Player
    {
        get
        {
            if (player == null)
            {
                OnInitialize();
            }
            return player;
        }
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        player = FindAnyObjectByType<Player>();
    }

}
