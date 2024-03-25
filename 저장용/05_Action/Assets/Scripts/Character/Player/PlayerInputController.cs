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

    PlayerInputAction inputAction;

    private void Awake()
    {
        inputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputAction.Player.Enable();
        inputAction.Player.Move.performed += OnMove;
        inputAction.Player.Move.canceled += OnMove;
        inputAction.Player.MoveModeChange.performed += OnMoveModeChage;
        inputAction.Player.Attack.performed += OnAttack;
        inputAction.Player.PickUp.performed += OnPickUp;
    }

    private void OnDisable()
    {
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Player.Move.canceled -= OnMove;
        inputAction.Player.MoveModeChange.performed -= OnMoveModeChage;
        inputAction.Player.Attack.performed -= OnAttack;
        inputAction.Player.PickUp.performed -= OnPickUp;
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
}