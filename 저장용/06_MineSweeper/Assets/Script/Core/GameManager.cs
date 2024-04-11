using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : Singltrun<GameManager>
{
    //  ���°���==================================
    public enum GameState 
    {
        Ready,
        Play,
        GameClear,
        GameOver
    }
    public Action onGameReady;
    public Action onGamePlay;
    public Action onGameClear;
    public Action onGameOver;

    GameState state = GameState.Ready;

    GameState State 
    {
        get => state;
        set 
        {
            if (state != value) 
            {
                state = value;
                switch (state) 
                {
                    case GameState.Ready:
                        FlagCount = mineCount;
                        playerName = string.Empty;
                        onGameReady?.Invoke();
                        break;
                    case GameState.Play:
                        ActionCount = 0;
                        if (playerName == string.Empty) 
                        {
                            playerName = $"Player{(uint)DateTime.Now.GetHashCode()}";
                        }
                        onGamePlay?.Invoke();
                        break;
                    case GameState.GameClear:
                        onGameClear?.Invoke();
                        break;
                    case GameState.GameOver:
                        onGameOver?.Invoke();
                        break;
                }
            }
        }
    }

    public bool IsPlay => state == GameState.Play;

    public void GameStart() 
    {
        if(State == GameState.Ready)    State = GameState.Play;
    }
    public void GameReset() 
    {
        State = GameState.Ready;
    }
    public void GameOver() 
    {
        State = GameState.GameOver;
    }
    public void GameClear() 
    {
        State = GameState.GameClear;
    }

    //  �������====================================

    public int mineCount = 10;
    public int boardWidth = 8;
    public int boardHeight = 8;
    
    public Board Board => board;
    Board board;

    //  ��� ����===================================
    public int FlagCount 
    {
        get => flagCount;
        private set 
        {
            if (flagCount != value) 
            {
                flagCount = value;
                onFlagCountChange?.Invoke(flagCount);
            }
        }
    }
    public Action<int> onFlagCountChange;

    public void IncreaseFlagCount() 
    {
        FlagCount++;
    }

    public void DecreaseFlagCount()
    {
        FlagCount--;
    }

    int flagCount = 0;
    //  �ൿ����======================================

    public int ActionCount 
    {
        get => actionCount;
        private set 
        {
            if (actionCount != value) 
            {
                actionCount = value;
                onActionCountChange?.Invoke(actionCount);
            }
        }
    }
    public Action<int> onActionCountChange;
    int actionCount = -1;

    public void PlayerActionEnd() 
    {
        ActionCount++;
    }

    //  �÷��̾� ����=================================

    string playerName = string.Empty;
    PlayerNameInputField inputField;

    public void SetPlayerName(string name) 
    {
        playerName = name;
    }

    //  =============================================

    protected override void OnInitialize()
    {
        board = FindAnyObjectByType<Board>();
        board.Initialize(boardWidth, boardHeight, mineCount);
        FlagCount = mineCount;

        inputField = FindAnyObjectByType<PlayerNameInputField>();
        inputField.onPlayerNameSet += (name) => playerName = name;
    }

#if UNITY_EDITOR
    public void Test_SetFlagCount(int flagCount) 
    {
        FlagCount = flagCount;
    }

    public void Test_StateChange(GameState state) 
    {
        State = state;
    }

#endif
}
