using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyBase : RecycleObject
{
    [Header("Enemy data")]

    public float moveSpeed = 1.0f;
    public int maxHp = 1;
    public int score = 10;

    Action giveScore;
    Player player;

    int hp = 1;
    protected int HP 
    {
        get => hp;
        set 
        {
            hp = value;
            if (hp < 0.1)
            {
                hp = 0;
                OnDie();
            }
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        OnInitialized();
    }

    protected override void OnDisable()
    {
        if (player != null) 
        {
            giveScore -= PlayerAddScore;
            giveScore = null;
            player = null;
        }
        base.OnDisable();
    }

    void PlayerAddScore() 
    {
        player.AddScore(score);
    }

    // Update is called once per frame
    void Update()
    {
        OnMoveUpdate(Time.deltaTime);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Player")) 
        {
            HP--;
        }
    }

    protected virtual void OnDie() 
    {
        Factory.Instance.Getexpolsion(this.transform.position);
        giveScore?.Invoke();
        gameObject.SetActive(false);
    }

    protected virtual void OnInitialized()
    {
        if(player == null)
        {
            player = GameManager.Instance.Player;
        }
        if (player != null) 
        {
            giveScore += PlayerAddScore;
        }

        HP = maxHp;
    }

    protected virtual void OnMoveUpdate(float deltaTime) 
    {
        transform.Translate(deltaTime*moveSpeed*-transform.right, Space.World);
    }

}
