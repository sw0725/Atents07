using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ItemEquip : TestBase
{
    Player player;
    public float data = 10.0f;

    public ItemCode code = ItemCode.Ruby;
    public Transform target;

    private void Start()
    {
        player = GameManager.Instance.Player;
        Factory.Instance.MakeItem(ItemCode.IronSword);
        Factory.Instance.MakeItem(ItemCode.SliverSword);
        Factory.Instance.MakeItem(ItemCode.OldSword);
        Factory.Instance.MakeItem(ItemCode.KiteShield);
        Factory.Instance.MakeItem(ItemCode.RoundShield);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        player.HP += data;
        player.MP += data;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        player.HP -= data;
        player.MP -= data;
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        player.HealthRegernerate(data, 1);
        player.ManaRegernerate(data, 1);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        player.HealthRegernerateByTick(3, 0.5f, 4);
        player.ManaRegernerateByTick(3, 0.5f, 4);
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        Factory.Instance.MakeItems(code, 1, target.position, true);
    }
}
