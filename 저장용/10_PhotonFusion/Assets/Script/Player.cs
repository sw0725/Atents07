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

    public override void FixedUpdateNetwork()   //��Ʈ��ũ ƽ���� ��� ����Ǵ� �Լ�
    {
        if (GetInput(out NetworkInputData data)) 
        {
            //data.direction.Normalize();

            cc.Move(Runner.DeltaTime * moveSpeed * data.direction); //�ʴ� ���꽺�ǵ��� �ӵ��� data�� �𷢼� �������� �̵�
            
            if(data.direction.sqrMagnitude > 0) 
            {
                forward = data.direction;       //ȸ�� ���� forward�������� ���� �߻�Ǵ� ���� ����
            }
        }
    }
}
