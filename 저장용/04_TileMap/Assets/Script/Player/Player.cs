using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    PlayerInputAction inputAction;
    Rigidbody2D rigid2d;
    Animator animator;

    Vector3 inputDir = Vector3.zero;
    float moveSpeed = 5f;

    private void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
        animator.SetFloat("InputY", inputDir.y);
        animator.SetFloat("InputX", inputDir.x);
        animator.SetBool("InputX", true);
        if (context.canceled) { animator.SetBool("InputX", false); }
    }

    private void Awake()
    {
        inputAction = new PlayerInputAction();
        animator = GetComponent<Animator>();
        rigid2d = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        inputAction.Player.Enable();
        inputAction.Player.Move.performed += OnMove;
        inputAction.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Player.Move.canceled -= OnMove;
        inputAction.Player.Disable();
    }

    private void FixedUpdate()
    {
        rigid2d.MovePosition(rigid2d.position + (Vector2)(Time.fixedDeltaTime * moveSpeed * inputDir));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
