using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayer : PlayerBase
{
    public float thinkingTimeMin = 1.0f;
    public float thinkingTimeMax = 5.0f;        //턴 듀레이션중 작은것 택

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
        StopAllCoroutines();        //타임아웃일때 코루틴 실행방지
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
//턴 시작시 랜덤 시간후에 자동공격