using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouns : EnemyBase
{
    [Header("Give PowerUp Enemy Data")]

    public float appearTime = 0.05f;
    public float WaitTime = 5.0f;
    public float SecondSpeed = 10.0f;
    public PoolObjectType bounsType = PoolObjectType.PowerUp;

    Animator animator;

    readonly int SpeedHash = Animator.StringToHash("Speed");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        StopAllCoroutines();
        StartCoroutine(AppearProcess());
    }

    IEnumerator AppearProcess()
    {
        animator.SetFloat(SpeedHash, moveSpeed);

        yield return new WaitForSeconds(appearTime);
        moveSpeed = 0.0f;
        animator.SetFloat(SpeedHash, moveSpeed);

        yield return new WaitForSeconds(WaitTime);
        moveSpeed = SecondSpeed;
        animator.SetFloat(SpeedHash, moveSpeed);
    }

    protected override void OnDie()
    {
        Factory.Instance.GetObject(bounsType, transform.position);
        base.OnDie();
    }


    //등장후 일정 시간 대기(ㅂ무빙-대기 반복) 죽을시 파워업 아이템 떨굼
}
