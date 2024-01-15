using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : EnemyBase
{

    Player player;

    Action gotIt;

    protected override void OnDisable()
    {
        if (player != null)
        {
            gotIt -= PlayerAddPower;
            gotIt = null;
            player = null;
        }
        base.OnDisable();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (player != null)
        {
            gotIt += PlayerAddPower;
        }

        HP = maxHp;
    }

    private void PlayerAddPower()
    {
        player.AddPower();
    }

    protected override void OnDie()
    {
        gotIt?.Invoke();
        gameObject.SetActive(false);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HP--;
        }
    }

    protected override void OnMoveUpdate(float deltaTime)
    {
        base.OnMoveUpdate(deltaTime);
    }
    ///하나 두줄
    ///두개 세줄
    ///새개 점수1000
    ///랜덤방향으로 움직임 일정시간마다 이동방향 바뀜 높은확률로 플레이어의 반대쪽으로 움직인다
}
