using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 0.01f;

    PlayerInputAction inputActions;
    Vector3 inputDir = Vector3.zero;

    Animator anim;
    Rigidbody rigid;

    private void Awake()
    {
        inputActions = new PlayerInputAction();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnStop;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnStop;

        inputActions.Player.Disable();
    }

    private void Start()
    {
        anim.SetBool("IsMove", false);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector3>();
        anim.SetBool("IsMove", true);
    }

    private void OnStop(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector3>();
        anim.SetBool("IsMove", false);
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + (Vector3)(Time.fixedDeltaTime * moveSpeed * inputDir));
    }
}
