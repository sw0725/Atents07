using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState : byte 
{
    Title = 0,
    ShipDeploy,
    Battle,
    GameEnd,
}

[RequireComponent(typeof(TurnControler))]
[RequireComponent(typeof(InputControler))]
public class GameManager : Singltrun<GameManager>
{
    //���� ���� ========================================

    GameState gameState = GameState.Title;
    public GameState GameState
    {
        get => gameState;
        set
        {
            if (gameState != value)
            {
                gameState = value;
                InputControler.ResetBind();
                onGameStateChage?.Invoke(gameState);
            }
        }
    }
    public Action<GameState> onGameStateChage;

    //�÷��̾�  ========================================
    public UserPlayer User => user;
    UserPlayer user;

    public EnemyPlayer Enemy => enemy;
    EnemyPlayer enemy;

    //��Ʈ�ѷ�  ========================================
    public TurnControler TurnManager => turnManager;
    TurnControler turnManager;

    public InputControler InputControler => input;
    InputControler input;

    //��Ÿ      ========================================

    public bool IsTestMode = false;

    CinemachineImpulseSource cameraImpulseSource;

    //==================================================

    protected override void OnpreInitialize()
    {
        base.OnpreInitialize();

        turnManager = GetComponent<TurnControler>();
        input = GetComponent<InputControler>();

        cameraImpulseSource = GetComponentInChildren<CinemachineImpulseSource>();
    }

    protected override void OnInitialize()
    {
        user = FindAnyObjectByType<UserPlayer>();
        enemy = FindAnyObjectByType<EnemyPlayer>();

        turnManager.OnInitialize(user, enemy);
    }

    public void CameraShake(float force) 
    {
        cameraImpulseSource.GenerateImpulseWithVelocity(force * UnityEngine.Random.insideUnitCircle.normalized);
    }
}
