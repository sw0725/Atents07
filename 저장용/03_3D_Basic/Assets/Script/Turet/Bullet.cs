using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : RecycleObject
{
    public float initialSpeed = 20.0f;
    public float lifeTime = 10.0f;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(LifeOver(lifeTime));
        rb.angularVelocity = Vector3.zero;              //재사용시 기존 회전력 초기화
        rb.velocity = initialSpeed * transform.forward;
    }

    private void OnCollisionEnter(Collision collision)
    {
        StopAllCoroutines();
        StartCoroutine(LifeOver(2.0f));
    }
    private void FixedUpdate()
    {                       //벡터의 길이 = 벡터가 0이 아닐때
        if(rb.velocity.sqrMagnitude>0.1f)
        transform.forward = rb.velocity;    //운동량은 중력의 영향으로 자동으로 아래를 향함 -> 을 총알의 벡터에 반영 -> 자연스러운 회전
    }
}

//public float initialSpeed = 20.0f;
//public float lifeTime = 10.0f;

//Rigidbody rb;

//private void Awake()
//{
//    rb = GetComponent<Rigidbody>();
//}

//private void Start()
//{                                   //velocity운동량(벡터)-어느방향의 어느정도로
//    rb.velocity = initialSpeed * transform.forward;

//    Destroy(this.gameObject, lifeTime);
//}
