using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerInputAction inputAction;
    public GameObject Bullet;

    Vector3 inputDir = Vector3.zero;
    float moveSpeed = 5f;
    private void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Bullet.SetActive(true);
        }
    }
    private void OnBoost(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Boost");
        }
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
    }

    private void Awake()
    {
        inputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputAction.Player.Enable();
        inputAction.Player.Fire.performed += OnFire;
        inputAction.Player.Fire.canceled += OnFire;
        inputAction.Player.Boost.performed += OnBoost;
        inputAction.Player.Boost.canceled += OnBoost;
        inputAction.Player.Move.performed += OnMove;
        inputAction.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        inputAction.Player.Fire.performed -= OnFire;
        inputAction.Player.Fire.canceled -= OnFire;
        inputAction.Player.Boost.performed -= OnBoost;
        inputAction.Player.Boost.canceled -= OnBoost;
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Player.Move.canceled -= OnMove;
        inputAction.Player.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime*moveSpeed*inputDir);
    }
}
