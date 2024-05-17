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

    //���� ================================================================

    ShipDeployData[] shipDeployDatas;

    public bool SaveShipDeployData()//�迭�� ���� �Է�
    {
        bool result = false;
        if (user.IsAllDeployed) 
        {
            shipDeployDatas = new ShipDeployData[user.Ships.Length];
            for(int i =0; i < shipDeployDatas.Length; i++) 
            {
                Ship ship = user.Ships[i];
                shipDeployDatas[i] = new ShipDeployData(ship.Direction, ship.Positions[0]);
            }
            result = true;
        }
        return result;
    }

    public bool LoadShipDeployData() //������� ��ġ
    {
        bool result = false;
        if(shipDeployDatas != null) 
        {
            user.UndoAllShipDeployment();
            for(int i =0; i<shipDeployDatas.Length; i++) 
            {
                Ship ship = user.Ships[i];
                ship.Direction = shipDeployDatas[i].Direction;
                user.Board.ShipDeployment(ship, shipDeployDatas[i].Position);
                ship.gameObject.SetActive(true);
            }
            result = true;
        }
        return result;
    }
}
