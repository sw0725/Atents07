using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    public float moveSpeed = 20.0f;

    [Networked]     //�� ������Ƽ�� ��ũ�εǾ��� == ��Ʈ��ũ���� �����Ǵ� ���̴�
    TickTimer Life { get; set; }

    public void Init()
    {
        Life = TickTimer.CreateFromSeconds(Runner, 5.0f);   //Life��5�ʸ� ����
    }



    public override void FixedUpdateNetwork()
    {
        if (Life.Expired(Runner))                           //5�ʰ� �����°�
        {
            Runner.Despawn(Object);
        }
        else 
        {
            transform.position += Runner.DeltaTime * moveSpeed * transform.forward;
        }
    }
}
