using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public Action<Vector2, bool> onMove;
    public Action onMoveModeChange;
    public Action onAttack;
    public Action onItemPickUp;
    public Action onLockOn;
    public Action onSkillStart;
    public Action onSkillEnd;

    PlayerInputAction inputAction;

    private void Awake()
    {
        inputAction = new PlayerInputAction();

        Player player = GetComponent<Player>();
        player.onDie += inputAction.Player.Disable;
    }

    private void OnEnable()
    {
        inputAction.Player.Enable();
        inputAction.Player.Move.performed += OnMove;
        inputAction.Player.Move.canceled += OnMove;
        inputAction.Player.MoveModeChange.performed += OnMoveModeChage;
        inputAction.Player.Attack.performed += OnAttack;
        inputAction.Player.PickUp.performed += OnPickUp;
        inputAction.Player.LockOn.performed += OnLockOn;
        inputAction.Player.Skill.performed += OnSkillStart;
        inputAction.Player.Skill.canceled += OnSkillEnd;
    }

    private void OnDisable()
    {
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Player.Move.canceled -= OnMove;
        inputAction.Player.MoveModeChange.performed -= OnMoveModeChage;
        inputAction.Player.Attack.performed -= OnAttack;
        inputAction.Player.PickUp.performed -= OnPickUp;
        inputAction.Player.LockOn.performed -= OnLockOn;
        inputAction.Player.Skill.performed -= OnSkillStart;
        inputAction.Player.Skill.canceled -= OnSkillEnd;
        inputAction.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector3 input = context.ReadValue<Vector2>();
        onMove?.Invoke(input, !context.canceled);
    }

    private void OnMoveModeChage(InputAction.CallbackContext context)
    {
        onMoveModeChange?.Invoke();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        onAttack?.Invoke();
    }

    private void OnPickUp(InputAction.CallbackContext _)
    {
        onItemPickUp?.Invoke();
    }

    private void OnLockOn(InputAction.CallbackContext context)
    {
        onLockOn?.Invoke();
    }
    private void OnSkillStart(InputAction.CallbackContext context)
    {
        onSkillStart?.Invoke();
    }
    private void OnSkillEnd(InputAction.CallbackContext context)
    {
        onSkillEnd?.Invoke();
    }
}