using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 0.01f;
    public float rotateSpeed = 180.0f;
    public float jumpPower = 6.0f;
    public float jumpCoolTime = 1.0f;

    float MoveDirection = 0.0f;                //1:전진 -1:후진 0:정지
    float RotateDirection = 0.0f;              //1:우회전 -1:좌회전 0:정지
    readonly int isMovehash = Animator.StringToHash("IsMove");
    readonly int isUsehash = Animator.StringToHash("Use");
    bool isjumping = false;
    float jumpTime = -1.0f;
    bool IsJumpAvailable => !isjumping && (jumpTime < 0.0f);

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

    private void OnMove(InputAction.CallbackContext context)        //context.started/perfrmed/canceled 로 따로 함수 안나눠도 시작/중단 분리가능
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
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * moveSpeed * MoveDirection * transform.forward);
    }

    void Rotate()
    {   //쿼터니언의Euler-xyz값만큼 회전 생성 |AngleAxis-특정축 기준 몇도만큼 회전 |FromToRatation-시작방향에서 도착방향이 될 수 있는 회전 생성
        //Lerp-시작회전에서 목표회전으로 보간(직선) |Slerp-러프와 동일(곡선) |LookRotation-특정방향을 바라보는 회전 생성 |identity-회전없음
        //Inverse-반전 |RotateTowards-시작값부터 목표값까지 -속도만큼 | 트랜스폼의 RotateAround-특정 위치에서 특정축 기준으로 회전 
        Quaternion rotate = Quaternion.AngleAxis(Time.fixedDeltaTime*rotateSpeed*RotateDirection, transform.up);
        rigid.MoveRotation(rigid.rotation * rotate);                                                      //기존회전값*회전값=포지션과 동일효과
    }   //리그바디의 무브로테이션은 포지션과 다르게 -만큼더가 아니라 -로설정한다. 부적합

    void Jump()
    {   //AddForce-피벗에가속 AddForceAtPosition-어떤 지점에 가속 AddExplosionForce-폭발구현 AddTorque-회전력
        if (IsJumpAvailable) 
        {
            rigid.AddForce(jumpPower * Vector3.up, ForceMode.Impulse);
            jumpTime = jumpCoolTime;
            isjumping = true;
        }
    }   //Force-기본(서서히,질량참고) Acceleration-기본(서서히,질량무시) Impulse-가속(한번에,질량참고) VelocityChange-가속(한번에,질량무시)

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Update()
    {
        jumpTime = -Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) 
        {
            isjumping = false;
        }
    }
}
