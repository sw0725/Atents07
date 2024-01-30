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
