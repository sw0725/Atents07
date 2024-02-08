using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : WayPointUser
{
    public float spinSpeed = 720.0f;

    Transform bladeMash;

    protected override Transform Target 
    {
        set 
        {
            base.Target = value;
            transform.LookAt(Target);
        }
    }

    private void Awake()
    {
        bladeMash = transform.GetChild(0);
    }

    private void Update()
    {
        bladeMash.Rotate(Time.deltaTime * spinSpeed * Vector3.right);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IAive aive = collision.gameObject.GetComponent<IAive>();
        if (aive != null) 
        {
            aive.Die();
        }
    }
}

//b: 웨이포인트 사용시 문제점 수정
//platfomBase: 특정 두지점을 계속 왕복하는 바닥-탑승시 플레이어 이동 주의
