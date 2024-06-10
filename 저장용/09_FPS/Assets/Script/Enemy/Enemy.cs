using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum HitLocation : byte 
{
    Body,
    Head,
    Arm,
    Leg
}

public class Enemy : MonoBehaviour
{
    //HP==========================================

    public float HP 
    {
        get => hp;
        set 
        {
            hp = value;
            if (hp <= 0) 
            {
                State = BehaviorState.Dead;
            }
        }
    }
    public float hp = 30.0f;
    public float MaxHP = 30.0f;

    public Action<Enemy> onDie;

    //����=========================================
    
    public enum BehaviorState : byte 
    {
        Wander = 0,
        Chase,
        Find,   //�������� �÷��̾ �þ߿��� ����� Ž���Ѵ�
        Attack, //�������� �����ѿ� ������ �����ֱ�� ����
        Dead    //�����簣 �Ŀ� �����
    }

    BehaviorState State 
    {
        get => state;
        set 
        {
            if(state != value) 
            {
                OnStateExit(state);
                state = value;
                OnStateEnter(value);
            }
        }
    }
    BehaviorState state = BehaviorState.Dead;

    Action onUpdate = null;

    //�̵�======================================

    public float walkSpeed = 2.0f;
    public float runSpeed = 7.0f;

    float speedPenalty = 0;             //�ٸ��λ�� ����

    //�þ�=======================================

    public float sightAngle = 90.0f;
    public float sightRange = 20.0f;

    //����======================================

    public float attackPower = 10.0f;
    public float attackInterval = 1.0f;

    float attackElapsed = 0;
    float attackPowerPenalty = 0;

    Player attackTarget = null;

    //Ž��=======================================

    public float findTime = 5.0f;
    public float findTimeElapsed = 0.0f;

    Transform chaseTarget = null;

    //��Ÿ=======================================

    NavMeshAgent agent;

    enum ItemTable : byte
    {
        Heal,
        AssaultRifle,
        ShotGun,
        Random
    }

    //===========================================

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        SphereCollider sphere = GetComponent<SphereCollider>();
        sphere.radius = sightRange;
    }

    private void OnEnable()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            chaseTarget = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chaseTarget = null;
        }
    }

    private void Update()
    {
        onUpdate();
    }

    private void Update_Wander()
    {
        if (FindPlayer()) 
        {
            State = BehaviorState.Chase;
        }
        else if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) 
        {
            agent.SetDestination(GetRandomDestination());
        }
    }

    private void Update_Chase()
    {
        if (IsPlayerInSight(out Vector3 pos))
        {
            agent.SetDestination(pos);
        }
        else if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            State = BehaviorState.Find;
        }
    }

    private void Update_Find()
    {

    }

    private void Update_Attack()
    {

    }

    private void Update_Dead()
    {

    }

    void OnStateEnter(BehaviorState state)
    {
        switch (state)
        {
            case BehaviorState.Wander:
                onUpdate = Update_Wander;
                agent.speed = walkSpeed;
                agent.SetDestination(GetRandomDestination());
                break;
            case BehaviorState.Chase:
                onUpdate = Update_Chase;
                agent.speed = runSpeed;
                break;
            case BehaviorState.Find:
                break;
            case BehaviorState.Attack:
                break;
            case BehaviorState.Dead:
                break;
        }
    }

    void OnStateExit(BehaviorState state)
    {
        switch (state) 
        {
            case BehaviorState.Find: 
                break;
            case BehaviorState.Attack:
                break;
            case BehaviorState.Dead:
                gameObject.SetActive(true);
                HP = MaxHP;
                break;
            default:
                break;
        }
    }

    Vector3 GetRandomDestination() 
    {
        int range = 3;

        Vector2Int current = MazeVisualizer.WorldToGrid(transform.position);
        int x = UnityEngine.Random.Range(current.x - range, current.x + range +1);
        int y = UnityEngine.Random.Range(current.y - range, current.y + range +1);

        return MazeVisualizer.GridToWorld(x, y);
    }

    void Attack() 
    {
    
    }

    public void OnAttacked(HitLocation hit, float damege) 
    {
        //��� ����
    }

    bool FindPlayer()           //�÷��̾� Ž�� �õ� true = ã��
    {
        bool result = false;
        if(chaseTarget != null) 
        {
            result = IsPlayerInSight(out _);
        }
        return result;
    }

    bool IsPlayerInSight(out Vector3 pos) //�÷��̾ �þ߹����ȿ� �ִ°�
    {
        bool result = false;
        pos = Vector3.zero;
        if (chaseTarget != null) 
        {
            Vector3 dir = chaseTarget.position - transform.position;
            Ray ray = new Ray(transform.position + Vector3.up * 1.9f, dir);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, sightRange, LayerMask.GetMask("Player", "Wall")))
            {
                if (hitInfo.transform == chaseTarget)
                {
                    float angle = Vector3.Angle(transform.forward, dir);    //������ �����(���������� ��ȯ)
                    if(angle * 2 < sightAngle)                              // = angle < sightAngle/2
                    {
                        pos = chaseTarget.position;
                        result = true;
                    }
                }
            }
        }
        return result;
    }

    IEnumerator LookAround() 
    {
        yield return null;
    }

    public void Respawn(Vector3 spawnPos) 
    {
        agent.Warp(spawnPos);
        State = BehaviorState.Wander;
    }

    void DropItem(ItemTable table = ItemTable.Random) 
    {
        
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        //��߰� �׸��� ���º��� �ٸ���
    }

    public Vector3 Test_GetRandomPosition() 
    {
        return GetRandomDestination();
    }

#endif
}
