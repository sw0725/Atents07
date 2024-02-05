using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float speed = 3.0f;

    Vector3 offset;                                     //�÷��̾�� ī�޶� ���� -> Ÿ����ġ+������=ī�޶� ��ġ
    float length;

    private void Start()
    {
        if (target == null) 
        {
            target = GameManager.Instance.Player.transform.GetChild(7);
        }

        offset = transform.position-target.position;    //Ÿ�ٿ��� �÷��̾� ���⺤��
        length = offset.magnitude;
    }

    private void FixedUpdate()                          //�÷��̾ �Ƚ������ �̹Ƿ� ������
    {
        transform.position = Vector3.Slerp(transform.position,     //Ÿ���� �󸶸�ŭ ȸ���ߴ°�    //�� ī�޶� ���⺤�Ϳ� �� = Ÿ��ȸ����ŭ ī�޶� ȸ��
            target.position + Quaternion.LookRotation(target.forward) * offset, Time.fixedDeltaTime * speed);
        transform.LookAt(target);                                   //ī�޶� Ÿ���� ������

        Ray ray = new Ray(target.position, transform.position - target.position);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, length)) 
        {
            transform.position = hitInfo.point;
        }
    }
}
