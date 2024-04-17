using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

public class NetPlayer : NetworkBehaviour
{
    public float moveSpeed = 3.5f;
    public float rotateSpeed = 90.0f;

    NetworkVariable<float> netMoveDir = new NetworkVariable<float>(0.0f);          //��Ʈ��ũ���� �����Ǵ� ����(��Ÿ�Ը� ����)
    NetworkVariable<float> netRotate = new NetworkVariable<float>(0.0f);

    PlayerInputAction action;
    CharacterController controller;
    Animator animator;

    enum AnimationState 
    {
        Idle =0,
        Walk,
        BackWalk,
        None
    }
    AnimationState state = AnimationState.None;

    NetworkVariable<AnimationState> netAnimationState;

    private void Awake()
    {
        action = new PlayerInputAction();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        netAnimationState.OnValueChanged += onAnimationStateChange;
    }

    private void OnEnable()
    {
        action.Player.Enable();
        action.Player.MoveForward.performed += OnMoveInput;
        action.Player.MoveForward.canceled += OnMoveInput;
        action.Player.Rotate.performed += OnRotateInput;
        action.Player.Rotate.canceled += OnRotateInput;
    }

    private void OnDisable()
    {
        action.Player.MoveForward.performed -= OnMoveInput;
        action.Player.MoveForward.canceled -= OnMoveInput;
        action.Player.Rotate.performed -= OnRotateInput;
        action.Player.Rotate.canceled -= OnRotateInput;
        action.Player.Disable();
    }

    private void Update()
    {
        if (netMoveDir.Value != 0.0f) 
        {
            controller.SimpleMove(netMoveDir.Value * transform.forward);
        }
        transform.Rotate(0, netRotate.Value * Time.deltaTime, 0, Space.World);
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        float moveInput = context.ReadValue<float>();           //Ű���� = -1,0,1 �� �ϳ�
        SetMoveInput(moveInput);
    }

    void SetMoveInput(float moveInput) 
    {
        float moveDir = moveInput * moveSpeed;                        //���������� ���� �����̴°��� �ٸ� ��Ʈ��ũ�� �˾ƾ� �� ��ġ�� �ٸ� ��Ʈ��ũ������ �����ϱ⿡ �� ���� ��Ʈ��ũ ���������� �����Ѵ�.
                                                                      //�̷������� ��Ʈ��ũ �󿡼� �����̴� ��ü���� NetworkObject �� �����߸� �Ѵ�.
        if (NetworkManager.Singleton.IsServer)                        //�ٸ� ���������� ���������� ���� �� �� �����Ƿ�
        {
            netMoveDir.Value = moveDir;
        }
        else if(IsOwner)
        {
            MoveRequestServerRpc(moveDir);                            //������ ������ �Ƿڸ� ������ �Ѵ�
        }

        if (moveDir > 0.001f)
        {
            state = AnimationState.Walk;
        }
        else if (moveDir < -0.001f)
        {
            state = AnimationState.BackWalk;
        }
        else
        {
            state = AnimationState.Idle;
        }
        if (state != netAnimationState.Value) 
        {
            if (NetworkManager.Singleton.IsServer)
            {
                netAnimationState.Value = state;
            }
            else 
            {
                UpdateAnimationStateServerRpc(state);
            }
        }
    }

    [ServerRpc]
    void MoveRequestServerRpc(float moveDir) 
    {
        netMoveDir.Value = moveDir;
    }

    [ServerRpc]
    void UpdateAnimationStateServerRpc(AnimationState state)
    {
        netAnimationState.Value = state;
    }

    private void OnRotateInput(InputAction.CallbackContext context)
    {
        float rotateInput = context.ReadValue<float>();           //Ű���� = -1,0,1 �� �ϳ�
        SetRotateInput(rotateInput);
    }

    void SetRotateInput(float rotateInput) 
    {
        float rotate = rotateInput * rotateSpeed;

        if (NetworkManager.Singleton.IsServer)
        {
            netRotate.Value = rotate;
        }
        else if(IsOwner)
        {
            RotateRequestServerRpc(rotate);
        }
    }

    [ServerRpc]
    void RotateRequestServerRpc(float rotate)
    {
        netRotate.Value = rotate;
    }

    private void onAnimationStateChange(AnimationState previousValue, AnimationState newValue)
    {
        animator.SetTrigger(newValue.ToString());
    }
}
