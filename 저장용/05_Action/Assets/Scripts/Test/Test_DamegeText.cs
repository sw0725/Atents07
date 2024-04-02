using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_DamegeText : TestBase
{
    Transform test;
    public float damege = 10;
    Player player;

    private void Awake()
    {
        test = transform.GetChild(0);
        player = GameManager.Instance.Player;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetDamegeText(10, test.position);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        player.Defence(damege);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Factory.Instance.GetEnemy();
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        Enemy enemy = FindAnyObjectByType<Enemy>();
        enemy.Defence(damege);
    }
}
