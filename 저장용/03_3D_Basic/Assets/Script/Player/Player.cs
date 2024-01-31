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

    float MoveDirection = 0.0f;                //1:���� -1:���� 0:����
    float RotateDirection = 0.0f;              //1:��ȸ�� -1:��ȸ�� 0:����
    readonly int isMovehash = Animator.StringToHash("IsMove");
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
            jumpTime = jumpCoolTime;
            isjumping = true;
        }
    }   //Force-�⺻(������,��������) Acceleration-�⺻(������,��������) Impulse-����(�ѹ���,��������) VelocityChange-����(�ѹ���,��������)

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
