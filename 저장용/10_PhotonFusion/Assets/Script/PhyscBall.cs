using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhyscBall : NetworkBehaviour
{
    public float moveSpeed = 20.0f;

    [Networked]     //�� ������Ƽ�� ��ũ�εǾ��� == ��Ʈ��ũ���� �����Ǵ� ���̴�
    TickTimer Life { get; set; }

    public void Init(Vector3 forward)
    {
        Life = TickTimer.CreateFromSeconds(Runner, 5.0f);   //Life��5�ʸ� ����
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = forward;
    }



    public override void FixedUpdateNetwork()
    {
        if (Life.Expired(Runner))                           //5�ʰ� �����°�
        {
            Runner.Despawn(Object);
        }
    }
}
