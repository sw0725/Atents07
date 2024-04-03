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
                        agent.isStopped = true;         //���� �������� ��ã�� ����
                        agent.velocity = Vector3.zero;  //��� 0
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
                        agent.isStopped = true;
                        agent.velocity = Vector3.zero;
                        attackCoolTime = attackSpeed;
                        onStateUpdate = Update_Attack;
                        break;
                    case EnemyState.Dead:
                        agent.isStopped = true;
                        agent.velocity = Vector3.zero;
                        animator.SetTrigger("Die");
                        onStateUpdate = Update_Dead;
                        break;
                }
            }
        }
    }
    Action onStateUpdate;   //=>�׼��� �Լ������ͷμ�(�Լ�����) Ȱ�� �� �����Ͱ� ����Ű�� �Լ��� ���Ǹ��� �ٲٸ� ������Ʈ���� �̰͸��� ȣ���ϴ°͸����� ����ġȿ���� �� �� �ִ�.

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

    public Waypoints waypoints;         //�����̺�ó�� ������

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
    public Action<int> onHit { get; set; }

    [System.Serializable]//����ü�� �����Ϳ��� ���� �ֵ���
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
    Transform hitPosition;

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

        c= transform.GetChild(4);
        AttackArea attackArea = c.transform.GetComponent<AttackArea>();
        attackArea.onPlayerIn += (target) => 
        {
            if (State == EnemyState.Chase) 
            {
                attackTarget = target;
                State = EnemyState.Attack;
            }
        };
        attackArea.onPlayerOut += (target) => 
        {
            if (attackTarget == target) 
            {
                attackTarget = null;
                if (State != EnemyState.Dead) 
                {
                    State = EnemyState.Chase;
                }
            } 
        };

        hitPosition = transform.GetChild(5);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        agent.speed = moveSpeed;            //Ʈ���Ŵ� Ʈ�������� �Ѿ�� True���ǰ� Ʈ�������� �Ѿ���� False �� �ȴ�.
                                            //�� �̹� true�� Ʈ���Ŵ� �ڽ��� �䱸���ϴ� Ʈ�������� ������ ���ϸ� True�� �����Ѵ�.
        State = EnemyState.Wait;            //�̶� True�� ������ StopƮ���Ŵ� �䱸�ϴ� Ʈ�������� Patrol->Wait�� �����Ƿ� False�� ���� ���ϰ�
        animator.ResetTrigger("Stop");      //���ʷ� Patrol�� �Ѿ���� �̹� ���� Stop Ʈ���Ű� Patrol->Wait �� �����鼭 Patrol�ִϸ��̼��� ����� ����Ǳ� ���� �ٷ� Wait�� �Ѿ �����°�
        HP = MaxHP;                         //Wait ���·� ��ȯ�ϸ鼭 stopƮ���Ű� ���ΰ��� �����Ͽ� �����۵��ϰ� �Ѵ�.

        rb.isKinematic = true;
        rb.drag = Mathf.Infinity;

        Player player = GameManager.Instance.Player;
        if (player != null) 
        {
            player.onDie += PlayerDie;
        }
    }                                       

    protected override void OnDisable()
    {
        if (GameManager.Instance != null) 
        {
            Player player = GameManager.Instance.Player;
            if (player != null)
            {
                player.onDie -= PlayerDie;
            }
        }

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
        if (SerchPlayer())
        {
            agent.SetDestination(chaseTarget.position);
        }
        else 
        {
            State = EnemyState.Wait;
        }
    }
    void Update_Attack() 
    {
        attackCoolTime -= Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(attackTarget.transform.position - transform.position), 0.1f);
        if (attackCoolTime < 0) 
        {
            Attack(attackTarget);
        }
    }
    void Update_Dead() 
    {
    
    }

    bool SerchPlayer()
    {
        bool result = false;
        chaseTarget = null;                                                 //���ݹ��� ��
        Collider[] colliders = Physics.OverlapSphere(transform.position, farSightRange, LayerMask.GetMask("Player")); //�迭�� ��ȯ�ǹǷ� �� ���̷� �ִ��� ������ �Ǵ�
        if(colliders.Length > 0)
        {
            Player player = colliders[0].GetComponent<Player>();
            if (player != null && player.IsAlive) 
            {
                Vector3 playerPos = colliders[0].transform.position;
                Vector3 toPlayerDir = playerPos - transform.position;
                if (toPlayerDir.sqrMagnitude < nearSightRange * nearSightRange)    //�������� ��
                {
                    chaseTarget = colliders[0].transform;
                    result = true;
                }
                else
                {
                    if (IsInSightAngle(toPlayerDir))     //�þ߰� ��
                    {
                        if (IsSightClear(toPlayerDir))   //���ع�����
                        {
                            chaseTarget = colliders[0].transform;
                            result = true;
                        }
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
        bool result = false;                    //���ʹ��� �����̿� ���߱�(�ǹ����� ������ ������� ���)
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
        animator.SetTrigger("Attack");
        target.Defence(AttackPower);
        Factory.Instance.GetEnemyHitEffect(hitPosition.position + UnityEngine.Random.insideUnitSphere * 0.1f);
        attackCoolTime = attackSpeed;
    }

    public void Defence(float damage)
    {
        if (IsAlive) 
        {
            animator.SetTrigger("Hit");
            float final = MathF.Max(0, (damage - DefencePower));
            HP -= final;
            onHit?.Invoke(Mathf.RoundToInt(final));
        }
    }

    public void Die()
    {
        //animator.ResetTrigger("Hit");             //�ִϸ��̼� �켱���������� ���̺��� ��Ʈ�� �켱�õǴ� ��Ȳ�߻� -> ������ ��Ʈ�� ��Ʈ�ִϸ� ����� -> �켱���� ������ �ذ�
        State = EnemyState.Dead;
        StartCoroutine(DeadSquence());
        onDie?.Invoke();
        onDie = null;
    }

    IEnumerator DeadSquence()                   //��������
    {
        bodycollider.enabled = false;
        dieEffect.Play();
        dieEffect.transform.SetParent(null);
        healthBar.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        MakeDropItems();
        yield return new WaitForSeconds(1.0f);  //�ִϸ޳��������� ���(��� �ִϸ� �ð� 1.5��)

        agent.enabled = false;                  //���ϸ� Ʈ������ ��ȭ�� �ȸ����� �ȳ�����
        rb.isKinematic = false;                 //�߷¿��� ��������(�ݶ��̴� ������ ������ ������)
        rb.drag = 10.0f;                        //õõ����������
        yield return new WaitForSeconds(2.0f);

        dieEffect.transform.SetParent(this.transform);
        dieEffect.transform.localPosition = EffectResetPosition;

        gameObject.SetActive(false);
    }

    void MakeDropItems() 
    {
        foreach (var item in dropItem) 
        {           //0-1���� ������ ����
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

    void PlayerDie() 
    {
        State = EnemyState.Wait;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() 
    {
        bool playerShow = SerchPlayer();
        Handles.color = playerShow ? Color.red : Color.green;

        Vector3 forward = transform.forward * farSightRange;
        Handles.DrawDottedLine(transform.position, transform.position + forward, 2.0f);     //�߽�

        Quaternion q1 = Quaternion.AngleAxis(-sightHalfRange, transform.up);
        Handles.DrawLine(transform.position, transform.position + q1 * forward);            //�߽ɼ� ȸ�� �� �׸���

        Quaternion q2 = Quaternion.AngleAxis(sightHalfRange, transform.up);
        Handles.DrawLine(transform.position, transform.position + q2 * forward);

        Handles.DrawWireArc(transform.position, transform.up, q1 * forward, sightHalfRange * 2, farSightRange, 2.0f);

        Handles.DrawWireDisc(transform.position, transform.up, nearSightRange);             //�ٰŸ� �þ߹���
    }

    public void Test_DropItem(int testCount) 
    {
        uint[] total = new uint[dropItem.Length];
        uint[] types = new uint[dropItem.Length];
        for (int i = 0; i<testCount; i++)
        {
            int index = 0;
            foreach (var item in dropItem)
            {           //0-1���� ������ ����
                if (UnityEngine.Random.value < item.dropRatio)
                {
                    uint count = (uint)UnityEngine.Random.Range(0, item.dropCount) + 1;
                    types[index]++;
                    total[index] += count;
                }
                index++;
            }
        }

        Debug.Log($"1st {types[0]}�� ���, {total[0]}��");
        Debug.Log($"2nd {types[0]}�� ���, {total[0]}��");
    }
#endif
}
