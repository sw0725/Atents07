using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singltrun<GameManager>
{
    Player player;
    public Player Player 
    {
        get 
        {
            if(player == null)
                player = FindAnyObjectByType<Player>();
            return player; 
        }
    }

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
    }
}
