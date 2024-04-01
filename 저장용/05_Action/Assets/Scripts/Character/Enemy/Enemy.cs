using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : RecycleObject, IBattler, IHealth
{
    protected enum EnemyState 
    {
        Wait = 0,
        Patrol,
        Chase,
        Attack,
        Dead
    }

    EnemyState state = EnemyState.Patrol;

    protected EnemyState State 
    {
        get => state;
        set 
        {
            if(state != value) 
            {
                state = value;
                switch (state) 
                {
                    case EnemyState.Wait:
                        agent.isStopped = true;         //현재 진행중인 길찾기 중지
                        agent.velocity = Vector3.zero;  //운동량 0
                        animator.SetTrigger("Stop");
                        WaitTimer = waitTime;
                        onStateUpdate = Update_Wait;
                        break;
                    case EnemyState.Patrol:
                        agent.isStopped = false;
                        agent.SetDestination(waypoints.NextTarget);
                        animator.SetTrigger("Move");
                        onStateUpdate = Update_Patrol;
                        break;
                    case EnemyState.Chase:
                        agent.isStopped = false;
                        animator.SetTrigger("Move");
                        onStateUpdate = Update_Chase;
                        break;
                    case EnemyState.Attack:
                        Debug.Log("attack");
                        onStateUpdate = Update_Attack;
                        break;
                    case EnemyState.Dead:
                        agent.isStopped = true;
                        agent.velocity = Vector3.zero;
                        animator.SetTrigger("Die");
                        Debug.Log("dead");
                        onStateUpdate = Update_Dead;
                        break;
                }
            }
        }
    }
    Action onStateUpdate;   //=>액션을 함수포인터로서(함수저장) 활용 이 포인터가 가르키는 함수를 조건마다 바꾸면 업데이트에서 이것만을 호출하는것만으로 스위치효과를 낼 수 있다.

    public float waitTime = 1.0f;
    float waitTimer = 1.0f;
    protected float WaitTimer 
    {
        get => waitTimer;
        set 
        {
            waitTimer = value;
            if (waitTimer < 0.0f) 
            {
                State = EnemyState.Patrol;
            }
        }
    }

    public float moveSpeed = 3.0f;

    public float farSightRange = 10.0f;
    public float sightHalfRange = 50.0f;
    public float nearSightRange = 1.5f;

    public Waypoints waypoints;         //프라이빗처럼 쓸것임

    public float AttackPower => attackPower;
    public float attackPower = 10.0f;
    public float attackSpeed = 1.0f;
    float attackCoolTime = 0.0f;
    public float DefencePower => defencePower;
    public float defencePower = 3.0f;

    public float HP 
    {
        get => hp;
        set 
        {
            hp = value;
            if (State != EnemyState.Dead && hp <= 0) 
            {
                Die();
            }
            hp = Mathf.Clamp(hp, 0, MaxHP);
            onHealthCange?.Invoke(hp/MaxHP);
        }
    }
    protected float hp = 100.0f;
    public float maxHP =100.0f;
    public float MaxHP => maxHP;

    public Action<float> onHealthCange { get; set; }

    public bool IsAlive => hp > 0;

    public Action onDie { get; set; }

    [System.Serializable]//구조체를 에디터에서 볼수 있도록
    public struct ItemDropInfo 
    {
        public ItemCode code;
        [Range(0,1)]
        public float dropRatio;
        public uint dropCount;
    }
    public ItemDropInfo[] dropItem;

    protected Transform chaseTarget = null;
    protected IBattler attackTarget = null;

    readonly Vector3 EffectResetPosition = new(0.0f, 0.01f, 0.0f);

    Animator animator;
    NavMeshAgent agent;
    SphereCollider bodycollider;
    Rigidbody rb;
    ParticleSystem dieEffect;
    EnemyHealthBar healthBar;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        bodycollider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();

        Transform c = transform.GetChild(2);
        healthBar = c.GetComponent<EnemyHealthBar>();
        c = transform.GetChild(3);
        dieEffect = c.GetComponent<ParticleSystem>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        agent.speed = moveSpeed;            //트리거는 트랜지션을 넘어갈때 True가되고 트랜지션을 넘어오면 False 가 된다.
                                            //즉 이미 true인 트리거는 자신을 요구로하는 트랜지션을 만나지 못하면 True를 유지한다.
        State = EnemyState.Wait;            //이때 True로 설정된 Stop트리거는 요구하는 트랜지션이 Patrol->Wait에 있으므로 False가 되지 못하고
        animator.ResetTrigger("Stop");      //최초로 Patrol로 넘어갔을때 이미 쌓인 Stop 트리거가 Patrol->Wait 을 만나면서 Patrol애니메이션이 제대로 재생되기 전에 바로 Wait로 넘어가 버리는것
        HP = MaxHP;                         //Wait 상태로 전환하면서 stop트리거가 쌓인것을 제거하여 정상작동하게 한다.

        rb.isKinematic = true;
        rb.drag = Mathf.Infinity;
    }                                       

    protected override void OnDisable()
    {
        bodycollider.enabled = true;
        healthBar.gameObject.SetActive(true);
        agent.enabled = true;

        base.OnDisable();
    }        

    private void Update()
    {
        onStateUpdate(); 
    }

    void Update_Wait() 
    {
        if (SerchPlayer())
        {
            State = EnemyState.Chase;
        }
        else
        {
            WaitTimer -= Time.deltaTime;
            Quaternion look = Quaternion.LookRotation(waypoints.NextTarget - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * 2);
        }
    }
    void Update_Patrol() 
    {
        if (SerchPlayer())
        {
            State = EnemyState.Chase;
        }
        else 
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                waypoints.StepNextWaypoint();
                State = EnemyState.Wait;
            }
        }
    }
    void Update_Chase() 
    {
        agent.SetDestination(chaseTarget.position);
        if (!SerchPlayer())
        {
            State = EnemyState.Wait;
        }
    }
    void Update_Attack() 
    {
        
    }
    void Update_Dead() 
    {
    
    }

    bool SerchPlayer()
    {
        bool result = false;
        chaseTarget = null;                                                 //원격범위 안
        Collider[] colliders = Physics.OverlapSphere(transform.position, farSightRange, LayerMask.GetMask("Player")); //배열은 반환되므로 그 길이로 있는지 없는지 판단
        if(colliders.Length > 0)
        {
            Vector3 playerPos = colliders[0].transform.position;
            Vector3 toPlayerDir = playerPos - transform.position;
            if (toPlayerDir.sqrMagnitude < nearSightRange * nearSightRange)    //근접범위 안
            {
                chaseTarget = colliders[0].transform;
                result = true;
            }
            else
            {
                if(IsInSightAngle(toPlayerDir))     //시야각 안
                {
                    if(IsSightClear(toPlayerDir))   //방해물없다
                    {
                        chaseTarget = colliders[0].transform;
                        result = true;
                    }
                }
            }
        }
        return result;
    }

    bool IsInSightAngle(Vector3 toTargetDirection) 
    {
        float angle = Vector3.Angle(transform.forward, toTargetDirection);
        return angle < sightHalfRange;
    }

    bool IsSightClear(Vector3 toTargetDirection)
    {
        bool result = false;                    //에너미의 눈높이에 맟추기(피벗에서 나가면 여러모로 곤란)
        Ray ray = new Ray(transform.position + transform.up * 0.5f, toTargetDirection);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, farSightRange))
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                result = true;
            }
        }
        return result;
    }

    public void Attack(IBattler target)
    {
        target.Defence(AttackPower);
    }

    public void Defence(float damage)
    {
        if (IsAlive) 
        {
            animator.SetTrigger("Hit");
            HP -= MathF.Max(0, damage - DefencePower);
        }
    }

    public void Die()
    {
        animator.ResetTrigger("Hit");
        State = EnemyState.Dead;
        StartCoroutine(DeadSquence());
        onDie?.Invoke();
    }

    IEnumerator DeadSquence()                   //사망연출용
    {
        bodycollider.enabled = false;
        dieEffect.Play();
        dieEffect.transform.SetParent(null);
        healthBar.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        MakeDropItems();
        yield return new WaitForSeconds(1.0f);  //애니메끝날때까지 대기(사망 애니메 시간 1.5초)

        agent.enabled = false;                  //안하면 트랜스폼 변화가 안먹혀서 안내려감
        rb.isKinematic = false;                 //중력영향 받을것임(콜라이더 없으니 땅으로 꺼진다)
        rb.drag = 10.0f;                        //천천히떨어지기
        yield return new WaitForSeconds(2.0f);

        dieEffect.transform.SetParent(this.transform);
        dieEffect.transform.localPosition = EffectResetPosition;

        gameObject.SetActive(false);
    }

    void MakeDropItems() 
    {
        foreach (var item in dropItem) 
        {           //0-1사이 랜덤값 생성
            if (UnityEngine.Random.value < item.dropRatio) 
            {
                uint count = (uint)UnityEngine.Random.Range(0, item.dropCount)+1;
                Factory.Instance.MakeItems(item.code, count, transform.position, true);
            }
        }
    }

    public void HealthRegernerate(float totalRegen, float duration)
    {
    }

    public void HealthRegernerateByTick(float tickRegen, float tickInterval, uint totalTickCount)
    {
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() 
    {
        bool playerShow = SerchPlayer();
        Handles.color = playerShow ? Color.red : Color.green;

        Vector3 forward = transform.forward * farSightRange;
        Handles.DrawDottedLine(transform.position, transform.position + forward, 2.0f);     //중심

        Quaternion q1 = Quaternion.AngleAxis(-sightHalfRange, transform.up);
        Handles.DrawLine(transform.position, transform.position + q1 * forward);            //중심선 회전 후 그리기

        Quaternion q2 = Quaternion.AngleAxis(sightHalfRange, transform.up);
        Handles.DrawLine(transform.position, transform.position + q2 * forward);

        Handles.DrawWireArc(transform.position, transform.up, q1 * forward, sightHalfRange * 2, farSightRange, 2.0f);

        Handles.DrawWireDisc(transform.position, transform.up, nearSightRange);
    }

    public void Test_DropItem(int testCount) 
    {
        uint[] total = new uint[dropItem.Length];
        uint[] types = new uint[dropItem.Length];
        for (int i = 0; i<testCount; i++)
        {
            int index = 0;
            foreach (var item in dropItem)
            {           //0-1사이 랜덤값 생성
                if (UnityEngine.Random.value < item.dropRatio)
                {
                    uint count = (uint)UnityEngine.Random.Range(0, item.dropCount) + 1;
                    types[index]++;
                    total[index] += count;
                }
                index++;
            }
        }

        Debug.Log($"1st {types[0]}번 드랍, {total[0]}개");
        Debug.Log($"2nd {types[0]}번 드랍, {total[0]}개");
    }
#endif
}
