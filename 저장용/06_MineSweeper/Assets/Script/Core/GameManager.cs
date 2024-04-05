using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singltrun<GameManager>
{
    //  상태관련==================================
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
                        onGameReady?.Invoke();
                        break;
                    case GameState.Play:
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

    //  깃발 관련===================================
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

    int flagCount = 0;
    //  ===========================================

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
