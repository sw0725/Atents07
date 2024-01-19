using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMissle : EnemyBase
{
    [Header("Missle Data")]
    public float guidePerformance =1.5f;

    Transform target;
    bool OnGuied = true;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        target = GameManager.Instance.Player.transform;
        OnGuied = true;
    }

    protected override void OnMoveUpdate(float deltaTime)
    {
        base.OnMoveUpdate(deltaTime);
        if (OnGuied) 
        {
            Vector3 dir = Vector3.left;
            if (target != null) 
            {
                dir = target.position - transform.position;
            }
            transform.right = -Vector3.Slerp(-transform.right, dir, deltaTime * guidePerformance); //시작각부터 도착각까지 프레임의 0.5속도로 움직인다
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (OnGuied && collision.gameObject.CompareTag("Player")) 
        {
            OnGuied = false;
        }
    }
}
