using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float walkSpeed = 3.0f;
    public float runSpeed = 5.0f;
    public float turnSpeed = 10.0f;
    [Range(0, AttackAnimationLength)]
    public float maxCoolTime = 0.3f;

    float currentSpeed = 0.0f;
    float coolTime = 0.0f;

    Transform weaponParent;
    Transform shiledParent;
    PlayerInputController inputController;
    Animator animator;
    CharacterController characterController;
    Action<bool> onWeaponEffectEnable;

    readonly int speed_Hash = Animator.StringToHash("Speed");
    readonly int attack_Hash = Animator.StringToHash("Attack");
    const float AnimatorStopSpeed = 0.0f;
    const float AnimatorWalkSpeed = 0.3f;
    const float AnimatorRunSpeed = 1.0f;
    const float AttackAnimationLength = 0.533f;

    enum MoveMode
    {
        Walk = 0,
        Run
    }
    MoveMode currentSpeedMode = MoveMode.Run;
    MoveMode CurrentSpeedMode
    {
        get => currentSpeedMode;
        set
        {
            currentSpeedMode = value;
            if (currentSpeed > 0.0f)
            {
                MoveSpeedChange(currentSpeedMode);
            }
        }
    }

    Vector3 inputDir = Vector3.zero;    //점프없음 - y=0
    Quaternion targetRotation = Quaternion.identity;    //바라볼 방향

    private void Awake()
    {
        Transform c = transform.GetChild(2);
        c = c.GetChild(0);
        c = c.GetChild(0);
        c = c.GetChild(0);
        
        Transform spine3 = c.GetChild(0);
        weaponParent = spine3.GetChild(2);
        weaponParent = weaponParent.GetChild(1);
        weaponParent = weaponParent.GetChild(0);
        weaponParent = weaponParent.GetChild(0);
        weaponParent = weaponParent.GetChild(2);
        
        shiledParent = spine3.GetChild(1);
        shiledParent = shiledParent.GetChild(1);
        shiledParent = shiledParent.GetChild(0);
        shiledParent = shiledParent.GetChild(0);
        shiledParent = shiledParent.GetChild(2);

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        inputController = GetComponent<PlayerInputController>();
        inputController.onMove += OnMoveInput;
        inputController.onMoveModeChange += OnMoveModeChangeInput;
        inputController.onAttack += OnAttackInput;
    }
    private void Start()
    {
        Weapon weapon = weaponParent.GetComponentInChildren<Weapon>();
        onWeaponEffectEnable += weapon.EffectEnable;
        ShowWeaponEffect(false);
    }

    private void Update()
    {
        characterController.Move(Time.deltaTime * currentSpeed * inputDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        coolTime -= Time.deltaTime;
    }

    private void OnAttackInput()
    {
        if(coolTime < 0 && ((currentSpeed < 0.001f)||(currentSpeedMode == MoveMode.Walk)))
        {
            animator.SetTrigger(attack_Hash);
            coolTime = maxCoolTime;
        }
    }

    private void OnMoveModeChangeInput()
    {
        if (CurrentSpeedMode == MoveMode.Walk)
        {
            CurrentSpeedMode = MoveMode.Run;
        }
        else
        {
            CurrentSpeedMode = MoveMode.Walk;
        }
    }

    private void OnMoveInput(Vector2 input, bool isPress)
    {
        inputDir.x = input.x;
        inputDir.y = 0;
        inputDir.z = input.y;

        if (isPress) //눌려짐
        {
            Quaternion camY = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0); //cam의 y축 회전 추출
            inputDir = camY * inputDir;                                     //입력 방향을 카메라의 y 회전과 같은 정도로 회전
            targetRotation = Quaternion.LookRotation(inputDir);         //몸체 회전

            MoveSpeedChange(CurrentSpeedMode);
        }
        else
        {
            currentSpeed = 0;
            animator.SetFloat(speed_Hash, AnimatorStopSpeed);
        }
    }

    void MoveSpeedChange(MoveMode mode)
    {
        switch (mode)
        {
            case MoveMode.Walk:
                currentSpeed = walkSpeed;
                animator.SetFloat(speed_Hash, AnimatorWalkSpeed);
                break;
            case MoveMode.Run:
                currentSpeed = runSpeed;
                animator.SetFloat(speed_Hash, AnimatorRunSpeed);
                break;
        }
    }

    public void ShowWeaponAndShield(bool isShow = true) 
    {
        weaponParent.gameObject.SetActive(isShow);
        shiledParent.gameObject.SetActive(isShow);
    }

    public void ShowWeaponEffect(bool isShow = true) 
    {
        onWeaponEffectEnable?.Invoke(isShow);
    }
}
