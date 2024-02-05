using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float speed = 3.0f;

    Vector3 offset;                                     //플레이어와 카메라 간격 -> 타겟위치+오프셋=카메라 위치
    float length;

    private void Start()
    {
        if (target == null) 
        {
            target = GameManager.Instance.Player.transform.GetChild(7);
        }

        offset = transform.position-target.position;    //타겟에서 플레이어 방향벡터
        length = offset.magnitude;
    }

    private void FixedUpdate()                          //플레이어가 픽스드업뎃 이므로 맞춰줌
    {
        transform.position = Vector3.Slerp(transform.position,     //타겟이 얼마만큼 회전했는가    //를 카메라 방향벡터에 곱 = 타겟회전만큼 카메라 회전
            target.position + Quaternion.LookRotation(target.forward) * offset, Time.fixedDeltaTime * speed);
        transform.LookAt(target);                                   //카메라가 타겟을 보도록

        Ray ray = new Ray(target.position, transform.position - target.position);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, length)) 
        {
            transform.position = hitInfo.point;
        }
    }
}
