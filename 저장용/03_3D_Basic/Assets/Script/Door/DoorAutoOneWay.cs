using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAutoOneWay : DoorAuto
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            Vector3 PlayerToDoor = transform.position - other.transform.position;       //�÷��̾�� ������ ���ϴ� ���⺤��
            float angle =  Vector3.Angle(transform.forward, PlayerToDoor);              //���� ���� ���� ����� ���� ���̰� �Ի�
            if(angle > 90.0f)                                                           //���̰��� 90���� ũ�� = ���� ���ֺ����ִ�
                Open();
        }
    }
}
