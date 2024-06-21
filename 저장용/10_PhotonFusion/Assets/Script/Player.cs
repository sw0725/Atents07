using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public float moveSpeed = 5.0f;

    Vector3 forward = Vector3.forward;

    NetworkCharacterController cc;

    private void Awake()
    {
        cc = GetComponent<NetworkCharacterController>();
    }

    public override void FixedUpdateNetwork()   //네트워크 틱별로 계속 실행되는 함수
    {
        if (GetInput(out NetworkInputData data)) 
        {
            //data.direction.Normalize();

            cc.Move(Runner.DeltaTime * moveSpeed * data.direction); //초당 무브스피드의 속도로 data의 디랙션 방향으로 이동
            
            if(data.direction.sqrMagnitude > 0) 
            {
                forward = data.direction;       //회전 도중 forward방향으로 공이 발사되는 것을 방지
            }
        }
    }
}
