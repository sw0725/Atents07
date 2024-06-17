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

    //상태=========================================
    
    public enum BehaviorState : byte 
    {
        Idle = 0,
        Wander,
        Chase,
        Find,   //추적도중 플레이어가 시야에서 벗어나면 탐색한다
        Attack, //추적도중 범위한에 들어오면 일정주기로 공격
        Dead,   //일정시간 후에 재생성
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

    [ColorUsage(false, true)]   //알파값과 hdr값 사용 여부
    public Color[] stateEyeColors;

    Material eyeColor;

    readonly int EyeColorID = Shader.PropertyToID("_BaceColor");

    //이동======================================

    public float walkSpeed = 2.0f;
    public float runSpeed = 7.0f;

    float speedPenalty = 0;             //다리부상시 증가

    //시야=======================================

    public float sightAngle = 90.0f;
    public float sightRange = 20.0f;

    //공격======================================

    public float attackPower = 10.0f;
    public float attackInterval = 1.0f;

    float attackElapsed = 0;
    float attackPowerPenalty = 0;

    Player attackTarget = null;

    AttackSensor attackSensor;

    //탐색=======================================

    public float findTime = 5.0f;
    public float findTimeElapsed = 0.0f;

    Transform chaseTarget = null;

    //기타=======================================

    NavMeshAgent agent;

    public enum ItemTable : byte
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

        Transform c = transform.GetChild(1);
        attackSensor = c.GetComponent<AttackSensor>();
        attackSensor.onSensorTriggered += (target) => 
        {
            if(attackTarget == null)
            {
                attackTarget = target.GetComponent<Player>();
                attackTarget.onDie += ReturnWander;
                State = BehaviorState.Attack;
            }
        };

        c =transform.GetChild(0);
        c = c.GetChild(0);
        c = c.GetChild(0);

        Renderer rand = c.GetComponent<Renderer>();
        eyeColor = rand.material;
        eyeColor.SetColor(EyeColorID, stateEyeColors[(int)BehaviorState.Wander]);

        onUpdate = Update_Idle;
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
    private void Update_Idle()
    {

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
        findTimeElapsed += Time.deltaTime;
        if (findTimeElapsed > findTime)
        {
            State = BehaviorState.Wander;
        } 
        else if (FindPlayer()) 
        {
            State = BehaviorState.Chase;
        }
    }

    private void Update_Attack()
    {
        agent.SetDestination(attackTarget.transform.position);

        Quaternion target = Quaternion.LookRotation(attackTarget.transform.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime);

        attackElapsed += Time.deltaTime;
        if(attackElapsed > attackInterval) 
        {
            Attack();
            attackElapsed = 0.0f;
        }
    }

    private void Update_Dead()
    {

    }

    void OnStateEnter(BehaviorState state)
    {
        eyeColor.SetColor(EyeColorID, stateEyeColors[(int)state]);
        switch (state)
        {
            case BehaviorState.Idle:
                onUpdate = Update_Idle;
                agent.speed = 0.0f;
                attackSensor.gameObject.SetActive(false);
                break;
            case BehaviorState.Wander:
                onUpdate = Update_Wander;
                agent.speed = walkSpeed * (1 - speedPenalty);
                agent.SetDestination(GetRandomDestination());
                break;
            case BehaviorState.Chase:
                onUpdate = Update_Chase;
                agent.speed = runSpeed * (1 - speedPenalty);
                break;
            case BehaviorState.Find:
                onUpdate = Update_Find;
                findTimeElapsed = 0.0f;
                agent.speed = walkSpeed * (1 - speedPenalty);
                agent.angularSpeed = 360.0f;
                StartCoroutine(LookAround());
                break;
            case BehaviorState.Attack:
                onUpdate= Update_Attack;
                break;
            case BehaviorState.Dead:
                DropItem();
                onUpdate= Update_Dead;
                agent.speed = 0.0f;
                agent.velocity = Vector3.zero;
                onDie?.Invoke(this);                //부활 요청
                gameObject.SetActive(false);
                break;
        }
    }

    void OnStateExit(BehaviorState state)
    {
        switch (state)
        {
            case BehaviorState.Idle:
                agent.speed = walkSpeed;
                attackSensor.gameObject.SetActive(true);
                break;
            case BehaviorState.Find:
                agent.angularSpeed = 120.0f;
                StopAllCoroutines();
                break;
            case BehaviorState.Attack:
                attackTarget.onDie -= ReturnWander;
                attackTarget = null;
                break;
            case BehaviorState.Dead:
                gameObject.SetActive(true);
                HP = MaxHP;
                speedPenalty = 0.0f;
                attackPowerPenalty = 0.0f;
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
        Debug.Log("attack to player");
        attackTarget.OnAttacked(this);
    }

    public void OnAttacked(HitLocation hit, float damege) 
    {
        HP -= damege;
        switch (hit) 
        {
            case HitLocation.Body:
                Debug.Log("Body Hit");
                break;
            case HitLocation.Head:
                HP -= damege;
                Debug.Log("Head Hit");
                break;
            case HitLocation.Arm:
                attackPowerPenalty += 0.1f;
                Debug.Log("Arm Hit");
                break;
            case HitLocation.Leg:
                speedPenalty += 0.3f;
                Debug.Log("Leg Hit");
                break;
        }

        if(State == BehaviorState.Wander || State == BehaviorState.Find) 
        {
            State = BehaviorState.Chase;
            agent.SetDestination(GameManager.Instance.Player.transform.position);
        }
        else 
        {
            agent.speed = runSpeed * (1 - speedPenalty);    
        }
    }

    void ReturnWander() 
    {
        State = BehaviorState.Wander;
    }

    bool FindPlayer()           //플레이어 탐색 시도 true = 찾음
    {
        bool result = false;
        if(chaseTarget != null) 
        {
            result = IsPlayerInSight(out _);
        }
        return result;
    }

    bool IsPlayerInSight(out Vector3 pos) //플레이어가 시야범위안에 있는가
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
                    float angle = Vector3.Angle(transform.forward, dir);    //무조건 양수값(작은각으로 반환)
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
        Vector3[] positions =
        {
            transform.position + transform.forward * 1.5f,
            transform.position - transform.forward * 1.5f,
            transform.position + transform.right * 1.5f,
            transform.position - transform.right * 1.5f,
        };

        int index = 0;
        int prev = 0;
        int length = positions.Length;
        while (true) 
        {
            do
            {
                index = UnityEngine.Random.Range(0, length);
            } while (index == prev);
            agent.SetDestination(positions[index]);
            prev = index;
            yield return new WaitForSeconds(1);
        }
    }

    public void Respawn(Vector3 spawnPos, bool init = false) 
    {
        agent.Warp(spawnPos);
        if (init)
        {
            State = BehaviorState.Idle;
        }
        else 
        {
            State = BehaviorState.Wander;
        }
    }

    void DropItem(ItemTable table = ItemTable.Random) 
    {
        ItemTable select = table;
        if (table == ItemTable.Random) 
        {
            float rand = UnityEngine.Random.value;
            if (rand < 0.8f) 
            {
                select = ItemTable.Heal;
            }
            else if (rand < 0.9f)
            {
                select = ItemTable.AssaultRifle;
            }
            else
            {
                select = ItemTable.ShotGun;
            }
        }
        Factory.Instance.GetDropItem(select, transform.position);
    }

    public void Play()
    {
        State = BehaviorState.Wander;
    }

    public void Stop() 
    {
        State = BehaviorState.Idle;
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        //사야각 그리기 상태별로 다른색
    }

    public Vector3 Test_GetRandomPosition() 
    {
        return GetRandomDestination();
    }

    public void Test_StateChange(BehaviorState state) 
    {
        State = state;
        agent.speed = 0;
        agent.velocity = Vector3.zero;
    }

    public void Test_EnemyStop() 
    {
        agent.speed = 0.0f;
        agent.velocity = Vector3.zero;
    }
#endif
}
