using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayer : PlayerBase
{
    public float thinkingTimeMin = 1.0f;
    public float thinkingTimeMax = 5.0f;        //�� �෹�̼��� ������ ��

    protected override void Start()
    {
        base.Start();

        opponent = gameManager.User;
        thinkingTimeMax = Mathf.Min(thinkingTimeMax, turnManager.TurnDuration);
    }

    protected override void OnPlayerTurnStart(int _)
    {
        base.OnPlayerTurnStart(_);
        float delay = Random.Range(thinkingTimeMin, thinkingTimeMax);
        StartCoroutine(AutoStart(delay));
    }

    protected override void OnPlayerTurnEnd()
    {
        StopAllCoroutines();        //Ÿ�Ӿƿ��϶� �ڷ�ƾ �������
        base.OnPlayerTurnEnd();
    }

    IEnumerator AutoStart(float delay) 
    {
        yield return new WaitForSeconds(delay);
        AutoAttack();
    }

    protected override void OnDefeat()
    {
        StopAllCoroutines();
        base.OnDefeat();
    }
}
//�� ���۽� ���� �ð��Ŀ� �ڵ�����