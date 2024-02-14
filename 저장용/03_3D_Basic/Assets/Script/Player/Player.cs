using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IAive
{
    public float moveSpeed = 0.01f;
    public float rotateSpeed = 180.0f;
    public float jumpPower = 6.0f;
    public float jumpCoolTime = 1.0f;
    public float startLiftime = 10.0f;
    public Action OnDie;
    public Action<float> OnLifeTimeChange;
    public Action<float> onJumpCoolTimeChange;

    float MoveDirection = 0.0f;                //1:���� -1:���� 0:����
    float RotateDirection = 0.0f;              //1:��ȸ�� -1:��ȸ�� 0:����
    float currentMoveSpeed = 5.0f;
    readonly int isMovehash = Animator.StringToHash("IsMove");
    readonly int isUsehash = Animator.StringToHash("Use");
    readonly int isDiehash = Animator.StringToHash("IsDie");
    float jumpTime = -1.0f;
    float JumpTime 
    {
        get => jumpTime;
        set 
        {
            jumpTime = value;
            onJumpCoolTimeChange?.Invoke(jumpTime/jumpCoolTime);
        }
    }
    bool IsJumpAvailable => !InAir && (jumpTime < 0.0f) && isAlive;
    bool isAlive = true;
    bool InAir 
    {
        get => GroundCount<1;
    }
    int groundCount = 0;
    int GroundCount 
    {
        get => groundCount;
        set
        {
            if (groundCount < 0) { groundCount = 0; }
            groundCount = value;
            if (groundCount < 0) { groundCount = 0; }
        }
    }

    float lifeTime = 0.0f;
    float LifeTime 
    {
        get => lifeTime;
        set 
        {
            lifeTime = value;
            if (lifeTime < 0.0f) 
            {
                lifeTime = 0.0f;
                Die(); 
            }
            OnLifeTimeChange?.Invoke(lifeTime/startLiftime);                //��ü�� ���ۼ�Ʈ�ΰ�
        }
    }

    PlayerInputAction inputActions;
    Animator anim;
    Rigidbody rigid;

    private void Awake()
    {
        inputActions = new PlayerInputAction();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        ItemUseChecker checker = GetComponentInChildren<ItemUseChecker>();
        checker.onItemUse += (inter) => inter.Use();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Use.performed += OnUse;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Use.performed -= OnUse;

        inputActions.Player.Disable();
    }

    private void Start()
    {
        currentMoveSpeed = moveSpeed;
        LifeTime = startLiftime;

        VirtualStick stick = GameManager.Instance.Stick;
        if (stick != null) 
        {
            stick.onMoveInput += (inputDelta) => SetInput(inputDelta, inputDelta.sqrMagnitude > 0.0025f);
            OnDie += stick.Stop;
        }

        VirtualButton botton = GameManager.Instance.Button;
        if (botton != null)
        {
            botton.onJumpInput += Jump;
            onJumpCoolTimeChange += botton.RefreashCoolTime;
            OnDie += botton.Stop;
        }
    }

    private void OnMove(InputAction.CallbackContext context)        //context.started/perfrmed/canceled �� ���� �Լ� �ȳ����� ����/�ߴ� �и�����
    {
        SetInput(context.ReadValue<Vector2>(), !context.canceled);
    }

    private void OnJump(InputAction.CallbackContext _)
    {
        Jump();
    }

    private void OnUse(InputAction.CallbackContext context)
    {
        anim.SetTrigger(isUsehash);
    }

    void SetInput(Vector2 input, bool isMove) 
    {
        MoveDirection = input.y;
        RotateDirection = input.x;

        anim.SetBool(isMovehash, isMove);
    }

    void Move() 
    {
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * currentMoveSpeed * MoveDirection * transform.forward);
    }

    void Rotate()
    {   //���ʹϾ���Euler-xyz����ŭ ȸ�� ���� |AngleAxis-Ư���� ���� ���ŭ ȸ�� |FromToRatation-���۹��⿡�� ���������� �� �� �ִ� ȸ�� ����
        //Lerp-����ȸ������ ��ǥȸ������ ����(����) |Slerp-������ ����(�) |LookRotation-Ư�������� �ٶ󺸴� ȸ�� ���� |identity-ȸ������
        //Inverse-���� |RotateTowards-���۰����� ��ǥ������ -�ӵ���ŭ | Ʈ�������� RotateAround-Ư�� ��ġ���� Ư���� �������� ȸ�� 
        Quaternion rotate = Quaternion.AngleAxis(Time.fixedDeltaTime*rotateSpeed*RotateDirection, transform.up);
        rigid.MoveRotation(rigid.rotation * rotate);                                                      //����ȸ����*ȸ����=�����ǰ� ����ȿ��
    }   //���׹ٵ��� ��������̼��� �����ǰ� �ٸ��� -��ŭ���� �ƴ϶� -�μ����Ѵ�. ������

    void Jump()
    {   //AddForce-�ǹ������� AddForceAtPosition-� ������ ���� AddExplosionForce-���߱��� AddTorque-ȸ����
        if (IsJumpAvailable) 
        {
            rigid.AddForce(jumpPower * Vector3.up, ForceMode.Impulse);
            JumpTime = jumpCoolTime;
        }
    }   //Force-�⺻(������,��������) Acceleration-�⺻(������,��������) Impulse-����(�ѹ���,��������) VelocityChange-����(�ѹ���,��������)

    public void Die() 
    {
        if (isAlive)
        {
            anim.SetTrigger(isDiehash);
            inputActions.Player.Disable();

            rigid.constraints = RigidbodyConstraints.None;    //������ �����ǰ� �����ִ� �Ӽ�, �̳����� �Ǿ��־� ����� ���� ���ڰ� �����ִ�. ���� ������ 0
            Transform head = transform.GetChild(0);

            rigid.AddForceAtPosition(-transform.forward, head.position, ForceMode.Impulse);
            rigid.AddTorque(transform.up * 1.5f, ForceMode.Impulse);
            GameManager.Instance.GameOver();
            OnDie?.Invoke();

            isAlive = false;
        }
    }

    public void SetSpeedModifier(float ratio =1.0f)
    {
        currentMoveSpeed = moveSpeed * ratio;
    }

    public void RestoreMoveSpeed() 
    {
        currentMoveSpeed = moveSpeed;
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Update()
    {
        JumpTime -= Time.deltaTime;
        LifeTime -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) 
        {
            GroundCount++;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            GroundCount--;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlatformBase platform = other.GetComponent<PlatformBase>();
        if (platform != null) 
        {
            platform.onMove += OnRideMovingObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlatformBase platform = other.GetComponent<PlatformBase>();
        if (platform != null)
        {
            platform.onMove -= OnRideMovingObject;
        }
    }

    void OnRideMovingObject(Vector3 delta) 
    {
        rigid.MovePosition(rigid.position + delta);
    }
}
