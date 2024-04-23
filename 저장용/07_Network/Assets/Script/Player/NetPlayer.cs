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
    public GameObject bulletPrefab;
    public GameObject OrbPrefab;

    NetworkVariable<float> netMoveDir = new NetworkVariable<float>(0.0f);          //��Ʈ��ũ���� �����Ǵ� ����(��Ÿ�Ը� ����)
    NetworkVariable<float> netRotate = new NetworkVariable<float>(0.0f);
    NetworkVariable<FixedString512Bytes> chatString = new NetworkVariable<FixedString512Bytes>();

    PlayerInputAction action;
    CharacterController controller;
    Animator animator;
    Transform fireTransform;

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

        fireTransform = transform.GetChild(4);
    }

    private void OnEnable()
    {
        action.Player.Enable();
        action.Player.MoveForward.performed += OnMoveInput;
        action.Player.MoveForward.canceled += OnMoveInput;
        action.Player.Rotate.performed += OnRotateInput;
        action.Player.Rotate.canceled += OnRotateInput;
        action.Player.Attack1.performed += OnAttack1;
        action.Player.Attack2.performed += OnAttack2;
    }

    private void OnDisable()
    {
        action.Player.MoveForward.performed -= OnMoveInput;
        action.Player.MoveForward.canceled -= OnMoveInput;
        action.Player.Rotate.performed -= OnRotateInput;
        action.Player.Rotate.canceled -= OnRotateInput;
        action.Player.Attack1.performed -= OnAttack1;
        action.Player.Attack2.performed -= OnAttack2;
        action.Player.Disable();
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner) 
        {
            GameManager.Instance.VCam.Follow = transform.GetChild(0);
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsOwner) 
        {
            GameManager.Instance.onPlayerDisconnected?.Invoke();
            GameManager.Instance.VCam.Follow = null;
        }
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
        if (IsOwner)                                                  
        {
            float moveDir = moveInput * moveSpeed;                   //���������� ���� �����̴°��� �ٸ� ��Ʈ��ũ�� �˾ƾ� �� ��ġ�� �ٸ� ��Ʈ��ũ������ �����ϱ⿡ �� ���� ��Ʈ��ũ ���������� �����Ѵ�.
                                                                     //�̷������� ��Ʈ��ũ �󿡼� �����̴� ��ü���� NetworkObject �� �����߸� �Ѵ�.
            if (NetworkManager.Singleton.IsServer)                   //�ٸ� ���������� ���������� ���� �� �� �����Ƿ�
            {
                netMoveDir.Value = moveDir;
            }
            else
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
        float rotateInput = context.ReadValue<float>();           //Ű���� = -1,0,1 �� �ϳ�
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

    private void OnAttack2(InputAction.CallbackContext context)     //�� ��ų
    {
        Attack2();
    }

    private void OnAttack1(InputAction.CallbackContext context)     //�� �Ѿ�
    {
        Attack1();
    }

    void Attack1() 
    { 
    
    }

    void Attack2()
    {
        GameObject orb = Instantiate(OrbPrefab, fireTransform.position, fireTransform.rotation);
    }
}
