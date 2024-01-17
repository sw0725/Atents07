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
    public int powerBonus = 1000;

    PlayerInputAction inputAction;
    Animator animator;
    Transform[] firePlace;
    GameObject flash;
    WaitForSeconds flashWait;

    Vector3 inputDir = Vector3.zero;
    float moveSpeed = 5f;
    IEnumerator fireCoroutine;
    private const float fireAngle = 30.0f;
    private int power = 1;
    private const int maxPower = 3;
    private const int minPower = 1;

    private int Power
    {
        get { return power; }
        set
        {
            if (power != value)
            {
                power = value;
                if (power > maxPower)
                {
                    AddScore(powerBonus);
                }
                power = Mathf.Clamp(power, minPower, maxPower);
                RefreshFirePositions();
            }
        }
    }

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
            for (int i = 0; i < Power; i++)
            {
                Fire(firePlace[i]);
            }
            yield return new WaitForSeconds(fireInterval);
        }
    }

    void Fire(Transform fireTransform) //총알 하나 발사
    {
        StartCoroutine(FlashEffect());
        Factory.Instance.GetBullet(fireTransform.position, fireTransform.eulerAngles.z);
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

    private void RefreshFirePositions() 
    {
        for (int i = 0; i < maxPower; i++) 
        {
            if (i < Power)
            {
                float startAngle = (Power - 1) * (fireAngle * 0.5f); //power 에따른 가장 높은 줄의 각
                float angle = (i * -fireAngle); //해당 시작각도부터의 아래로 내려가는 각도
                firePlace[i].rotation = Quaternion.Euler(0,0, startAngle + angle);

                firePlace[i].localPosition = Vector3.zero;
                firePlace[i].Translate(0.5f, 0.0f, 0.0f);

                firePlace[i].gameObject.SetActive(true);
            }
            else 
            {
                firePlace[i].gameObject.SetActive(false);
            }
        }    
    }

#if UNITY_EDITOR    //유니티 에디터에서 실행할때만 실행
    public void Test_PowerUP() 
    {
        Power++;
    }

    public void Test_PowerDown()
    {
        Power--;
    }
#endif

    private void Awake()
    {
        inputAction = new PlayerInputAction();
        animator = GetComponent<Animator>();
        Transform fireRoot =transform.GetChild(0);
        firePlace = new Transform[fireRoot.childCount];
        for (int i = 0; i < firePlace.Length; i++)
        {
            firePlace[i] = fireRoot.GetChild(i);
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PowerUp")) 
        {
            Power++;
            collision.gameObject.SetActive(false);
        }
    }
}
