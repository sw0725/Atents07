using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAutoOneWay : DoorAuto
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            Vector3 PlayerToDoor = transform.position - other.transform.position;       //플레이어에서 문으로 향하는 방향벡터
            float angle =  Vector3.Angle(transform.forward, PlayerToDoor);              //문이 앞을 보는 방향과 벡터 사이각 게산
            if(angle > 90.0f)                                                           //사이각이 90보다 크다 = 문을 마주보고있다
                Open();
        }
    }
}
