using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    public float moveSpeed = 20.0f;

    [Networked]     //이 프로퍼티는 싱크로되었다 == 네트워크에서 공유되는 값이다
    TickTimer Life { get; set; }

    public void Init()
    {
        Life = TickTimer.CreateFromSeconds(Runner, 5.0f);   //Life는5초를 샌다
    }



    public override void FixedUpdateNetwork()
    {
        if (Life.Expired(Runner))                           //5초가 지낫는가
        {
            Runner.Despawn(Object);
        }
        else 
        {
            transform.position += Runner.DeltaTime * moveSpeed * transform.forward;
        }
    }
}
