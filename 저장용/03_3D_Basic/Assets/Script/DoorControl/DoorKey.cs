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
//    {                                         //ex ��Ӱ���
//        target = door;              //as = ���� ���ǰ��� �ɼ� �ִ°�(��/null) is = ���� ���ǰ��� �ɼ� �ִ°�(T/F)  //ĳ���� = ������ Ÿ�� ��ȯ
//    }                               //door��DoorAuto�� ĳ���� �����ϸ� ĳ���� �ƴϸ� null
//}                                   //targer = door.��������Ʈ<DoorAuto>-����

// ������ Ÿ�� ���� ������ �����, �̿������� ȸ����

// ��������� ���� DoorAutoLock : DoorAuto  
// ��ݰ� ������ ������ ��ȭ
// UnlockDoorKey : ����� ����� ����
