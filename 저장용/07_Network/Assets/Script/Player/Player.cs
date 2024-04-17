using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public float rotateSpeed = 90.0f;

    float moveDir = 0.0f;
    float rotate = 0.0f;

    PlayerInputAction action;
    CharacterController controller;
    Animator animator;

    enum AnimationState
    {
        Idle = 0,
        Walk,
        BackWalk,
        None
    }
    AnimationState state = AnimationState.None;
    AnimationState State
    {
        get => state;
        set
        {
            if (value != state)
            {
                state = value;
                animator.SetTrigger(state.ToString());
            }
        }
    }

    private void Awake()
    {
        action = new PlayerInputAction();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
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
        controller.SimpleMove(moveDir * transform.forward);
        transform.Rotate(0, rotate * Time.deltaTime, 0, Space.World);
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        float moveInput = context.ReadValue<float>();           //키보드 = -1,0,1 중 하나
        moveDir = moveInput * moveSpeed;

        if (moveDir > 0.001f)
        {
            State = AnimationState.Walk;
        }
        else if (moveDir < -0.001f)
        {
            State = AnimationState.BackWalk;
        }
        else
        {
            State = AnimationState.Idle;
        }
    }

    private void OnRotateInput(InputAction.CallbackContext context)
    {
        float rotateInput = context.ReadValue<float>();           //키보드 = -1,0,1 중 하나
        rotate = rotateInput * rotateSpeed;
    }
}
