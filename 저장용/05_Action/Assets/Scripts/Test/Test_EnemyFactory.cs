using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_EnemyFactory : TestBase
{

    private void Start()
    {
        Player player = GameManager.Instance.Player;
        player.Inventory.AddItem(ItemCode.IronSword);
        player.Inventory[0].EquipItem(player.gameObject);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetEnemy();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Factory.Instance.GetEnemy(1, Vector3.zero);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Enemy enemy = FindAnyObjectByType<Enemy>();
        enemy.HP -= 1000;
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
    }
}
