using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Test_PlayerBase : TestBase
{
    public Button reset;
    public Button random;
    public Button resetAndRandom;

    public PlayerBase user;
    public PlayerBase enemy;

    private void Start()
    {
        user = GameManager.Instance.User;
        enemy = GameManager.Instance.Enemy;

        reset.onClick.AddListener( user.Clear );
        reset.onClick.AddListener ( enemy.Clear );

        bool isShow = GameManager.Instance.IsTestMode;

        random.onClick.AddListener(() => 
        {
            user.AutoShipDeployment(true);
            enemy.AutoShipDeployment(isShow);
        });

        resetAndRandom.onClick.AddListener(() =>
        {
            user.Clear();
            user.AutoShipDeployment(true);

            enemy.Clear();
            enemy.AutoShipDeployment(isShow);
        });

        user.AutoShipDeployment(true);
        enemy.AutoShipDeployment(isShow);
    }

    protected override void OnLClick(InputAction.CallbackContext context)//공
    {
        Vector2Int grid = user.Board.GetMouseGridPosition();
        enemy.Attack(grid);
        grid = enemy.Board.GetMouseGridPosition();
        user.Attack(grid);
    }

    protected override void OnRClick(InputAction.CallbackContext context)//배치헤제
    {
        Vector2Int grid = user.Board.GetMouseGridPosition();
        ShipType type = user.Board.GetShipTypeOnBoard(grid);
        if(type != ShipType.None) 
        {
            UserPlayer userPlayer = user as UserPlayer;
            userPlayer.UndoShipDeploy(type);
        }

        Vector2Int enemygrid = enemy.Board.GetMouseGridPosition();
        user.Test_IsSuccessLine(enemygrid);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        user.AutoAttack();
    }
}
