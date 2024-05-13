using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Battle : TestBase
{
    UserPlayer user;
    EnemyPlayer enemy;

    private void Start()
    {
        GameManager gameManager = GameManager.Instance;

        user = gameManager.User;
        enemy = gameManager.Enemy;

        user.AutoShipDeployment(true);
        enemy.AutoShipDeployment(true);

        gameManager.GameState = GameState.Battle;
        user.Test_BindInputSys();

        gameManager.TurnManager.onTurnStart += (_) => user.AutoAttack();
        user.AutoAttack();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        enemy.AutoAttack();
    }

    protected override void OnLClick(InputAction.CallbackContext context)
    {
        Vector2Int grid = user.Board.GetMouseGridPosition();
        enemy.Attack(grid);
    }
}
