using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    public float fireInterval = 0.5f;
    public Action<int> onScoreChange;

    PlayerInputAction inputAction;
    Animator animator;
    Transform firePlace;
    GameObject flash;
    WaitForSeconds flashWait;

    Vector3 inputDir = Vector3.zero;
    float moveSpeed = 5f;
    IEnumerator fireCoroutine;
    public int PowerScale = 1;
    int score = 0;

    public int Score
    {
        get => score;
        private set 
        {
            if (score != value) 
            {
                score = Math.Min(value, 99999);
                onScoreChange?.Invoke(score);
            }
        }
    }
    private void OnFireStart(InputAction.CallbackContext _)
    {
        StartCoroutine(fireCoroutine);
    }
    private void OnFireEnd(InputAction.CallbackContext context) 
    {
        StopCoroutine(fireCoroutine);
    }

    IEnumerator FireCoroutine() 
    {
        while (true) 
        {
            Fire(firePlace.position);
            yield return new WaitForSeconds(fireInterval);
        }
    }

    void Fire(Vector3 positon, float angle = 0.0f) //총알 하나 발사
    {
        StartCoroutine(FlashEffect());
        switch (PowerScale) 
        {
            case 1:
                Factory.Instance.GetBullet(positon, angle);
                break;
            case 2:
                Factory.Instance.GetBullet(positon, 15.0f);
                Factory.Instance.GetBullet(positon, -15.0f);
                break;
            case 3:
                Factory.Instance.GetBullet(positon, 30.0f);
                Factory.Instance.GetBullet(positon, angle);
                Factory.Instance.GetBullet(positon, -30.0f);
                break;
        }
    }
    IEnumerator FlashEffect() 
    {
        flash.SetActive(true);
        
        yield return flashWait;

        flash.SetActive(false);
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
        animator.SetFloat("InputY", inputDir.y);
    }

    public void AddScore(int getScore) 
    {
        Score += getScore;
    }

    public void AddPower()
    {
        if (PowerScale < 4) { PowerScale++; }
    }

    private void Awake()
    {
        inputAction = new PlayerInputAction();
        animator = GetComponent<Animator>();
        firePlace =transform.GetChild(0);
        flash = transform.GetChild(1).gameObject;
        flashWait = new WaitForSeconds(0.1f);
        fireCoroutine = FireCoroutine();
    }

    private void OnEnable()
    {
        inputAction.Player.Enable();
        inputAction.Player.Fire.performed += OnFireStart;
        inputAction.Player.Fire.canceled += OnFireEnd;
        inputAction.Player.Boost.performed += OnBoost;
        inputAction.Player.Boost.canceled += OnBoost;
        inputAction.Player.Move.performed += OnMove;
        inputAction.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        inputAction.Player.Fire.performed -= OnFireStart;
        inputAction.Player.Fire.canceled -= OnFireEnd;
        inputAction.Player.Boost.performed -= OnBoost;
        inputAction.Player.Boost.canceled -= OnBoost;
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Player.Move.canceled -= OnMove;
        inputAction.Player.Disable();
    }

    private void FixedUpdate()
    {
        transform.Translate(Time.fixedDeltaTime * moveSpeed * inputDir);
    }
}
