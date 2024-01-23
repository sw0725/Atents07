using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class Test_GameOver : TestBase
{
    public int Score;
    public RankLine RankLine;
    public RankPanel rankPanel;

    Player player;

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        player.Test_Die();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        player.Test_SetScore(Score);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        RankLine.SetData("°¡°¡°¡", 10000);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        rankPanel.Test_RankPanel();
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        rankPanel.Test_SaveRankPanel();
    }
}
