using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputControler : MonoBehaviour
{
    public Action<Vector2> onMouseMove;
    public Action<Vector2> onMouseClick;
    public Action<float> onMouseWheel;

    PlayerInputAction action;

    private void Awake()
    {
        action = new PlayerInputAction();
    }

    private void OnEnable()
    {
        action.Player.Enable();
        action.Player.Click.performed += OnClick;
        action.Player.Move.performed += OnMove;
        action.Player.Wheel.performed += OnWheel;
    }

    private void OnDisable()
    {
        action.Player.Click.performed -= OnClick;
        action.Player.Move.performed -= OnMove;
        action.Player.Wheel.performed -= OnWheel;
        action.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        onMouseMove?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnClick(InputAction.CallbackContext _)
    {
        onMouseClick?.Invoke(Mouse.current.position.ReadValue());
    }

    private void OnWheel(InputAction.CallbackContext context)
    {
        onMouseWheel?.Invoke(context.ReadValue<float>());
    }

    public void ResetBind()
    {
        onMouseMove = null;
        onMouseClick = null;
        onMouseWheel = null;
    }
}
