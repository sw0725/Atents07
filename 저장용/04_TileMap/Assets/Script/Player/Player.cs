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

    Vector2 inputDir = Vector2.zero;
    float currentAtteckCoolTime = 0.0f;
    float currentSpeed= 3.0f;
    bool isMove = false;

    bool IsAtteckReady => currentAtteckCoolTime < 0.0f;

    readonly int inputX_Hash = Animator.StringToHash("InputX");
    readonly int inputY_Hash = Animator.StringToHash("InputY");
    readonly int isMove_Hash = Animator.StringToHash("IsMove");
    readonly int Atteck_Hash = Animator.StringToHash("Atteck");

    PlayerInputAction inputAction;
    Rigidbody2D rigid2d;
    Animator animator;

    private void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
        animator.SetFloat(inputY_Hash, inputDir.y);
        animator.SetFloat(inputX_Hash, inputDir.x);

        isMove = true;
        animator.SetBool(isMove_Hash, isMove);
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
        }
    }

    public void RestorSpeed()
    {
        currentSpeed = moveSpeed;
    }

    private void Awake()
    {
        inputAction = new PlayerInputAction();
        animator = GetComponent<Animator>();
        rigid2d = GetComponent<Rigidbody2D>();
        currentSpeed = moveSpeed;
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

    private void FixedUpdate()
    {
        rigid2d.MovePosition(rigid2d.position + (Vector2)(Time.fixedDeltaTime * currentSpeed * inputDir));
    }

    private void Update()
    {
        currentAtteckCoolTime -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
