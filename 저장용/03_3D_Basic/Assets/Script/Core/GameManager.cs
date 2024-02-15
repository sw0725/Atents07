using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singltrun<GameManager>
{
    public Action onClear;
    public Action onOver;
    public bool IsPlaying => !isClear && !isOver;

    bool isClear = false;
    bool isOver = false;

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

    VirtualStick stick;
    public VirtualStick Stick 
    {
        get 
        {
            if (stick == null)
                stick = FindAnyObjectByType<VirtualStick>();
            return stick;
        }
    }

    VirtualButton button;
    public VirtualButton Button 
    {
        get 
        {
            if (button == null)
                button = FindAnyObjectByType<VirtualButton>();
            return button;
        }
    }

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        stick = FindAnyObjectByType<VirtualStick>();
        button = FindAnyObjectByType<VirtualButton>();
    }

    public void GameClear() 
    {
        if (!isClear) 
        {
            onClear?.Invoke();
            isClear = true;
        }
    }

    public void GameOver() 
    {
        if (!isOver) 
        {
            onOver?.Invoke();
            isOver = true;
        }
    }
}
