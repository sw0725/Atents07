using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    public float fireInterval = 0.5f;
    public Action<int> onScoreChange;
    public Action<int> onLifeChange;
    public Action<int> onDead;
    public int powerBonus = 1000;
    public float invincibleTime = 2.0f;

    PlayerInputAction inputAction;
    Rigidbody2D rigid2d;
    Animator animator;
    Transform[] firePlace;
    GameObject flash;
    WaitForSeconds flashWait;
    SpriteRenderer spriteRenderer;

    Vector3 inputDir = Vector3.zero;
    float moveSpeed = 5f;
    IEnumerator fireCoroutine;
    private const float fireAngle = 30.0f;
    private int power = 1;
    private const int maxPower = 3;
    private const int minPower = 1;
    private bool IsAlive => life >0;

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

    private int life = 3;
    private const int startLife = 3;

    int Life 
    {
        get => life;
        set 
        {
            if (life != value) 
            {
                life = value;
                if (IsAlive)
                {
                    OnHit();
                }
                else
                {
                    OnDie();
                }

                life = Mathf.Clamp(life, 0, startLife);
                onLifeChange?.Invoke(life);
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

    void Fire(Transform fireTransform) //�Ѿ� �ϳ� �߻�
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
                float startAngle = (Power - 1) * (fireAngle * 0.5f); //power ������ ���� ���� ���� ��
                float angle = (i * -fireAngle); //�ش� ���۰��������� �Ʒ��� �������� ����
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

    private void OnHit()
    {
        Power--;
        Power = math.max(1, Power);
        StartCoroutine(Invincible());
    }

    void OnDie() 
    {
        Collider2D body = GetComponent<Collider2D>();
        body.enabled = false;       //�ش�������Ʈ�� ����

        Factory.Instance.Getexpolsion(transform.position);

        inputAction.Player.Disable();

        rigid2d.gravityScale = 1.0f;
        rigid2d.freezeRotation = false;
        rigid2d.AddTorque(10000);
        rigid2d.AddForce(Vector2.left * 10.0f, ForceMode2D.Impulse); //�⺻ force:�̵� Impulse:����

        onDead?.Invoke(Score);
    }

    IEnumerator Invincible() 
    {
        gameObject.layer = LayerMask.NameToLayer("Invincible");
        float timer = 0.0f;
        while (timer < invincibleTime) 
        {
            timer += Time.deltaTime;
            float alpha = (Mathf.Cos(timer * 30.0f) + 1.0f) * 0.5f;       //cos = -1~1
            spriteRenderer.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        gameObject.layer = LayerMask.NameToLayer("Player");
        spriteRenderer.color = Color.white;
    }

#if UNITY_EDITOR    //����Ƽ �����Ϳ��� �����Ҷ��� ����
    public void Test_PowerUP() 
    {
        Power++;
    }

    public void Test_PowerDown()
    {
        Power--;
    }

    public void Test_Die()
    {
        Life = 0;
    }
    public void Test_SetScore(int score)
    {
        Score = score;
    }

#endif

    private void Awake()
    {
        inputAction = new PlayerInputAction();
        animator = GetComponent<Animator>();
        rigid2d = GetComponent<Rigidbody2D>();
        Transform fireRoot =transform.GetChild(0);
        firePlace = new Transform[fireRoot.childCount];
        for (int i = 0; i < firePlace.Length; i++)
        {
            firePlace[i] = fireRoot.GetChild(i);
        }
        flash = transform.GetChild(1).gameObject;
        flashWait = new WaitForSeconds(0.1f);
        fireCoroutine = FireCoroutine();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    private void Start()
    {
        Power = 1;
        Life = startLife;
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
        if (IsAlive)
        {
            rigid2d.MovePosition(rigid2d.position + (Vector2)(Time.fixedDeltaTime * moveSpeed * inputDir));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) 
        {
            Life--;
        }
        else if (collision.gameObject.CompareTag("PowerUp")) 
        {
            Power++;
            collision.gameObject.SetActive(false);
        }
    }
}
