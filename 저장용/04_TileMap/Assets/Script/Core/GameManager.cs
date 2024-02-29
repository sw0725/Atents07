using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
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

    WorldManager world;
    public WorldManager World => world;

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        world.Initialize();
    }

    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();
        world = GetComponent<WorldManager>();
        world.PreInitialize();
    }

    public bool showSlimePath = false;

    private void OnValidate()       //��������� ���� �ٲ𶧸���.
    {
        Slime[] slimes = FindObjectsOfType<Slime>();
        foreach (Slime slime in slimes)
        {
            slime.ShowPath(showSlimePath);
        }
    }
}
