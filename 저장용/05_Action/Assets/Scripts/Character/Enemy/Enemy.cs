using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IBattler, IHealth
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
                        Debug.Log("wait");
                        onStateUpdate = Update_Wait;
                        break;
                    case EnemyState.Patrol:
                        waypointTarget = waypoints.Current;
                        agent.SetDestination(waypointTarget.position);
                        Debug.Log("patrol");
                        onStateUpdate = Update_Patrol;
                        break;
                    case EnemyState.Chase:
                        Debug.Log("chase");
                        onStateUpdate = Update_Chase;
                        break;
                    case EnemyState.Attack:
                        Debug.Log("attack");
                        onStateUpdate = Update_Attack;
                        break;
                    case EnemyState.Dead:
                        Debug.Log("dead");
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

    public Waypoints waypoints;
    protected Transform waypointTarget = null;

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

    [System.Serializable]//����ü�� �����Ϳ��� ���� �ֵ���
    public struct ItemDropInfo 
    {
        public ItemCode code;
        [Range(0,1)]
        public float dropRatio;
    }
    public ItemDropInfo[] dropItem;


    protected Transform chaseTarget = null;
    protected IBattler attackTarget = null;

    Animator animator;
    NavMeshAgent agent;
    SphereCollider collider;
    Rigidbody rb;
    ParticleSystem dieEffect;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        collider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        //dieEffect = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        agent.speed = moveSpeed;
        if (waypoints == null) 
        {
            waypointTarget = transform;
        }
        else 
        {
            waypointTarget = waypoints.Current;
        }

        State = EnemyState.Wait;
    }

    private void Update()
    {
        onStateUpdate(); 
    }

    void Update_Wait() 
    {
        WaitTimer -= Time.deltaTime;
    }
    void Update_Patrol() 
    {
        if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
        {
            waypoints.GoNext();
            State = EnemyState.Wait;
        }
    }
    void Update_Chase() 
    {
    
    }
    void Update_Attack() 
    {
    
    }
    void Update_Dead() 
    {
    
    }

    public void Attack(IBattler target)
    {
    }

    public void Defence(float damage)
    {
    }

    public void Die()
    {
    }

    public void HealthRegernerate(float totalRegen, float duration)
    {
    }

    public void HealthRegernerateByTick(float tickRegen, float tickInterval, uint totalTickCount)
    {
    }
}
