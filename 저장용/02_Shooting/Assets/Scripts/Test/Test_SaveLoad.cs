using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_SaveLoad : TestBase
{
    public int Score;
    public RankPanel rankPanel;

    Player player;
# if UNITY_EDITOR
    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        rankPanel.Test_SaveRankPanel();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {

    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        player.Test_Die();
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        player.Test_SetScore(Score);
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
    }
# endif
}
