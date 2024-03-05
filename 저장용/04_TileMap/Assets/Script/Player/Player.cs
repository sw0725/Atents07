using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float atteckCoolTime = 1.0f;
    public float maxLifeTime = 10.0f;
    public Action<Vector2Int> onMapChange;
    public Action<float> onLifeTimeChange;      //float = 수명의 비율 life/max
    public Action<int> onKillCountChange;

    Vector2 inputDir = Vector2.zero;
    Vector2Int currntMap;
    Vector2Int CurrentMap 
    {
        get => currntMap;
        set 
        {
            if (value != currntMap) 
            {
                currntMap = value;
                onMapChange?.Invoke(currntMap);
            }
        }
    }
    float currentAtteckCoolTime = 0.0f;
    float currentSpeed= 3.0f;
    float lifeTime;
    float LifeTime 
    {
        get => lifeTime;
        set 
        {
            if (lifeTime != value) 
            {
                lifeTime = value;

                lifeTime = Mathf.Clamp(lifeTime, 0.0f, maxLifeTime);
                onLifeTimeChange?.Invoke(lifeTime/maxLifeTime);
            }
        }
    }
    bool isMove = false;
    bool isAtteckValid = false;
    int killCount = 0;
    int KillCount 
    {
        get => killCount;
        set 
        {
            if(killCount != value) 
            {
                killCount = value;
                onKillCountChange?.Invoke(killCount);
            }
        }
    }

    bool IsAtteckReady => currentAtteckCoolTime < 0.0f;

    readonly int inputX_Hash = Animator.StringToHash("InputX");
    readonly int inputY_Hash = Animator.StringToHash("InputY");
    readonly int isMove_Hash = Animator.StringToHash("IsMove");
    readonly int Atteck_Hash = Animator.StringToHash("Atteck");

    List<Slime> attackTargets;

    PlayerInputAction inputAction;
    Rigidbody2D rigid2d;
    Animator animator;
    Transform attackSensorAxis;
    WorldManager world;

    private void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
        animator.SetFloat(inputY_Hash, inputDir.y);
        animator.SetFloat(inputX_Hash, inputDir.x);

        isMove = true;
        animator.SetBool(isMove_Hash, isMove);

        AttackSensorRotate(inputDir);
    }

    private void OnStop(InputAction.CallbackContext _)
    {
        inputDir = Vector2.zero;

        isMove = false;
        animator.SetBool(isMove_Hash, isMove);
    }

    private void OnAtteck(InputAction.CallbackContext _)
    {
        if (IsAtteckReady) 
        {
            animator.SetTrigger(Atteck_Hash);
            currentAtteckCoolTime = atteckCoolTime;
            currentSpeed = 0;
            isAtteckValid = false;
        }
    }

    public void RestorSpeed()
    {
        currentSpeed = moveSpeed;
    }

    void AttackSensorRotate(Vector2 dir) 
    {
        if (dir.y < 0)
        {
            attackSensorAxis.rotation = Quaternion.identity;
        }
        else if (dir.y > 0)
        {
            attackSensorAxis.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (dir.x < 0)
        {
            attackSensorAxis.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (dir.x > 0)
        {
            attackSensorAxis.rotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            attackSensorAxis.rotation = Quaternion.identity;
        }
    }

    void AttackValid()  //공격 애니메중 애니메 이벤트를 통해 호출됨 
    {                   //유효시(공격 시작시) 작동
        isAtteckValid = true;
        foreach (var slime in attackTargets) //var = C#이 알아서 넣는 타입
        {
            slime.Die();
        }
        attackTargets.Clear();
    }

    void AttackNotValid() 
    {                   //무효시(공격동작 끝) 작동
        isAtteckValid = false;
    }

    public void MonsterKill(float bonus) 
    {
        LifeTime += bonus;
        KillCount++;
    }

    private void Awake()
    {
        inputAction = new PlayerInputAction();
        animator = GetComponent<Animator>();
        rigid2d = GetComponent<Rigidbody2D>();
        attackSensorAxis = transform.GetChild(0);
        
        currentSpeed = moveSpeed;

        attackTargets = new List<Slime>(4);
        AttackSensor sensor = attackSensorAxis.GetComponentInChildren<AttackSensor>();
        sensor.onEnemyEnter += (slime) =>
        {
            if (isAtteckValid)
            {
                slime.Die();
            }
            else 
            {
                attackTargets.Add(slime);
            }
            slime.ShowOutLine();
        };
        sensor.onEnemyExit += (slime) =>
        {
            attackTargets.Remove(slime);
            slime.ShowOutLine(false);
        };
    }

    private void OnEnable()
    {
        inputAction.Player.Enable();
        inputAction.Player.Move.performed += OnMove;
        inputAction.Player.Move.canceled += OnStop;
        inputAction.Player.Atteck.performed += OnAtteck;
    }

    private void OnDisable()
    {
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Player.Move.canceled -= OnStop;
        inputAction.Player.Atteck.performed -= OnAtteck;
        inputAction.Player.Disable();
    }

    private void Start()
    {
        world = GameManager.Instance.World;
        LifeTime = maxLifeTime;
    }

    private void FixedUpdate()
    {
        rigid2d.MovePosition(rigid2d.position + (Vector2)(Time.fixedDeltaTime * currentSpeed * inputDir));
        CurrentMap = world.WorldToGrid(rigid2d.position);
    }

    private void Update()
    {
        currentAtteckCoolTime -= Time.deltaTime;
        LifeTime -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
