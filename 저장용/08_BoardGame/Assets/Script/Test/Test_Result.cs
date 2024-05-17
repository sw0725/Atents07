using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Result : TestBase
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
        user.BindInputSys();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        user.AutoAttack();
        enemy.AutoAttack();
    }

    protected override void OnRClick(InputAction.CallbackContext context)
    {
        Vector2Int g = user.Board.GetMouseGridPosition();
        enemy.Attack(g);
    }
}
