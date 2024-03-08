using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public float walkSpeed = 3.0f;
    public float runSpeed = 5.0f;
    public float turnSpeed = 10.0f;
    
    float currentSpeed = 0.0f;

    enum MoveMode 
    {
        Walk = 0,
        Run
    }
    MoveMode currentSpeedMode = MoveMode.Run;
    MoveMode CurrentSpeedMode 
    {
        get => currentSpeedMode;
        set 
        {
            currentSpeedMode = value;
            if (currentSpeed > 0.0f) 
            {
                MoveSpeedChange(currentSpeedMode);
            }
        }
    }
    Vector3 inputDir = Vector3.zero;    //�������� - y=0
    Quaternion targetRotation = Quaternion.identity;    //�ٶ� ����

    PlayerInputAction inputAction;
    Animator animator;
    CharacterController characterController;

    readonly int speed_Hash = Animator.StringToHash("Speed");
    const float AnimatorStopSpeed = 0.0f;
    const float AnimatorWalkSpeed = 0.3f;
    const float AnimatorRunSpeed = 1.0f;

    private void Awake()
    {
        inputAction = new PlayerInputAction();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        inputAction.Player.Enable();
        inputAction.Player.Move.performed += OnMove;
        inputAction.Player.Move.canceled += OnMove;
        inputAction.Player.MoveModeChange.performed += OnMoveModeChage;
    }

    private void OnDisable()
    {
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Player.Move.canceled -= OnMove;
        inputAction.Player.MoveModeChange.performed -= OnMoveModeChage;
        inputAction.Player.Disable();
    }

    private void Update()
    {
        characterController.Move(Time.deltaTime * currentSpeed * inputDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector3 input = context.ReadValue<Vector2>();

        inputDir.x = input.x;
        inputDir.y = 0;
        inputDir.z = input.y;
        if (!context.canceled) //������
        {
            Quaternion camY = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0); //cam�� y�� ȸ�� ����
            inputDir = camY * inputDir;                                     //�Է� ������ ī�޶��� y ȸ���� ���� ������ ȸ��
            targetRotation = Quaternion.LookRotation(inputDir);         //��ü ȸ��

            MoveSpeedChange(CurrentSpeedMode);
        }
        else 
        {
            currentSpeed = 0;
            animator.SetFloat(speed_Hash, AnimatorStopSpeed);
        }
    }

    void MoveSpeedChange(MoveMode mode) 
    {
        switch (mode)
        {
            case MoveMode.Walk:
                currentSpeed = walkSpeed;
                animator.SetFloat(speed_Hash, AnimatorWalkSpeed);
                break;
            case MoveMode.Run:
                currentSpeed = runSpeed;
                animator.SetFloat(speed_Hash, AnimatorRunSpeed);
                break;
        }
    }

    private void OnMoveModeChage(InputAction.CallbackContext context)
    {
        if (CurrentSpeedMode == MoveMode.Walk)
        {
            CurrentSpeedMode = MoveMode.Run;
        }
        else 
        {
            CurrentSpeedMode = MoveMode.Walk;
        }
    }
}