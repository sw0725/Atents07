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
    ///�ϳ� ����
    ///�ΰ� ����
    ///���� ����1000
    ///������������ ������ �����ð����� �̵����� �ٲ� ����Ȯ���� �÷��̾��� �ݴ������� �����δ�
}
