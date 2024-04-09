using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    public Sprite[] buttonSprites;

    Button button;
    Image image;

    enum ButtonState 
    {
        Nomal,
        Surprise,
        GameClear,
        GmaeOver
    }
    ButtonState state = ButtonState.Nomal;
    ButtonState State 
    {
        get => state;
        set 
        {
            if (state != value) 
            {
                state = value;
                image.sprite = buttonSprites[(int)state];
            }
        }
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    private void Start()
    {
        button.onClick.AddListener(OnClick);

        GameManager gameManager = GameManager.Instance;
        gameManager.Board.onBoardLeftPress += () =>
        {
            State = ButtonState.Surprise;
        };
        gameManager.Board.onBoardLeftRelease += () =>
        {
            State = ButtonState.Nomal;
        };
        gameManager.onGameOver += () => 
        {
            State = ButtonState.GmaeOver;
        };
        gameManager.onGameClear += () => 
        {
            State = ButtonState.GameClear;
        };
    }

    private void OnClick()
    {
        State = ButtonState.Nomal;
        GameManager.Instance.GameReset();
    }
}
