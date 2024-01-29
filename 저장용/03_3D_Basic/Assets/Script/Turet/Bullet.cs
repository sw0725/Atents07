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
        rb.velocity = initialSpeed * transform.forward;
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
