using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : MonoBehaviour
{
    public float rotateSpeed = 360.0f;
    public Action onConsume;

    Transform modle;

    private void Awake()
    {
        modle = transform.GetChild(0);
    }

    private void Update()
    {
        modle.Rotate(Time.deltaTime * rotateSpeed * transform.up);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Player")) 
        {
            OnConsume();
        }
    }


    protected virtual void OnConsume() 
    {
        onConsume?.Invoke();
        Destroy(gameObject);
    }
}


//void ResetTarget(DoorBase door) 
//{
//    if (door != null) 
//    {                                         //ex 상속관계
//        target = door;              //as = 앞이 뒤의것이 될수 있는가(값/null) is = 앞이 뒤의것이 될수 있는가(T/F)  //캐스팅 = 데이터 타입 변환
//    }                               //door가DoorAuto로 캐스팅 가능하면 캐스팅 아니면 null
//}                                   //targer = door.겟컴포넌트<DoorAuto>-권장

// 닿을시 타겟 열림 닿을시 사라짐, 이오브제는 회전함

// 잠금해제용 열쇠 DoorAutoLock : DoorAuto  
// 잠금과 해제시 문색상 변화
// UnlockDoorKey : 연결된 오토락 해제
