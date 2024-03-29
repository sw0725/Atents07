using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_EnemyHit : TestBase
{
    Player player;

    public ItemCode code = ItemCode.Ruby;
    public Transform target;

    private void Start()
    {
        player = GameManager.Instance.Player;
        player.Inventory.AddItem(ItemCode.IronSword);
        player.Inventory[0].EquipItem(player.gameObject);
        //Factory.Instance.MakeItem(ItemCode.IronSword);
        //Factory.Instance.MakeItem(ItemCode.SliverSword);
        //Factory.Instance.MakeItem(ItemCode.OldSword);
        //Factory.Instance.MakeItem(ItemCode.KiteShield);
        //Factory.Instance.MakeItem(ItemCode.RoundShield);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetHitEffect(target.position);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        Factory.Instance.MakeItems(code, 1, target.position, true);
    }
}
