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
        rb.angularVelocity = Vector3.zero;              //����� ���� ȸ���� �ʱ�ȭ
        rb.velocity = initialSpeed * transform.forward;
    }

    private void OnCollisionEnter(Collision collision)
    {
        StopAllCoroutines();
        StartCoroutine(LifeOver(2.0f));
    }
    private void FixedUpdate()
    {                       //������ ���� = ���Ͱ� 0�� �ƴҶ�
        if(rb.velocity.sqrMagnitude>0.1f)
        transform.forward = rb.velocity;    //����� �߷��� �������� �ڵ����� �Ʒ��� ���� -> �� �Ѿ��� ���Ϳ� �ݿ� -> �ڿ������� ȸ��
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
//{                                   //velocity���(����)-��������� ���������
//    rb.velocity = initialSpeed * transform.forward;

//    Destroy(this.gameObject, lifeTime);
//}
