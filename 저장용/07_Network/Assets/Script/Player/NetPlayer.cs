using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetPlayer : NetworkBehaviour
{
    public float moveSpeed = 3.5f;
    public float rotateSpeed = 90.0f;

    NetworkVariable<float> netMoveDir = new NetworkVariable<float>(0.0f);          //네트워크에서 공유되는 변수(값타입만 가능)
    NetworkVariable<float> netRotate = new NetworkVariable<float>(0.0f);
    NetworkVariable<FixedString512Bytes> chatString = new NetworkVariable<FixedString512Bytes>();

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
    NetworkVariable<AnimationState> netAnimationState = new NetworkVariable<AnimationState>();

    private void Awake()
    {
        action = new PlayerInputAction();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        netAnimationState.OnValueChanged += onAnimationStateChange;
        chatString.OnValueChanged += OnChatRecive;
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
        float moveInput = context.ReadValue<float>();           //키보드 = -1,0,1 중 하나
        SetMoveInput(moveInput);
    }

    void SetMoveInput(float moveInput)
    {                    
        if (IsOwner)                                                  
        {
            float moveDir = moveInput * moveSpeed;                   //접속한이후 내가 움직이는것을 다른 네트워크가 알아야 내 위치를 다른 네트워크에서도 수정하기에 이 값을 네트워크 공유변수로 설정한다.
                                                                     //이런식으로 네트워크 상에서 움직이는 물체들은 NetworkObject 를 가져야만 한다.
            if (NetworkManager.Singleton.IsServer)                   //다만 공유변수는 서버에서만 값을 쓸 수 있으므로
            {
                netMoveDir.Value = moveDir;
            }
            else
            {
                MoveRequestServerRpc(moveDir);                            //서버에 값변경 의뢰를 보내야 한다
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
                if (IsServer)
                {
                    netAnimationState.Value = state;
                }
                else
                {
                    UpdateAnimationStateServerRpc(state);
                }
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
        float rotateInput = context.ReadValue<float>();           //키보드 = -1,0,1 중 하나
        SetRotateInput(rotateInput);
    }

    void SetRotateInput(float rotateInput) 
    {
        if (IsOwner)
        {
            float rotate = rotateInput * rotateSpeed;

            if (IsServer)
            {
                netRotate.Value = rotate;
            }
            else 
            {
                RotateRequestServerRpc(rotate);
            }
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

    public void SendChat(string message) 
    {
        if (IsServer) 
        {
            chatString.Value = message;
        }
        else 
        {
            ChatRequestServerRpc(message);
        }
    }

    private void OnChatRecive(FixedString512Bytes previousValue, FixedString512Bytes newValue)
    {
        GameManager.Instance.Log(newValue.ToString());
    }

    [ServerRpc]
    void ChatRequestServerRpc(FixedString512Bytes messege)
    {
        chatString.Value = messege;
    }
}
