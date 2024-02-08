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

//b: ��������Ʈ ���� ������ ����
//platfomBase: Ư�� �������� ��� �պ��ϴ� �ٴ�-ž�½� �÷��̾� �̵� ����
