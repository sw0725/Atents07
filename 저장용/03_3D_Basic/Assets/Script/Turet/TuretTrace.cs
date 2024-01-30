using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TuretTrace : TuretBase
{
    public float sightRange = 10.0f;
    public float turnSpeed = 2.0f;
    public float fireAngle = 10.0f;

    bool isFire = false;

    SphereCollider sightTrigger;
    Player target;
    Transform body;

    protected override void Awake()
    {
        base.Awake();
        sightTrigger = GetComponent<SphereCollider>();
        body = transform.GetChild(2);
    }

    private void Start()
    {
        sightTrigger.radius= sightRange;
    }

    private void Update()
    {
        LookTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == GameManager.Instance.Player.transform)       //싱글턴에서 생성한 객체이므로 단 하나뿐이다. 즉 비교가능
        {
            target = GameManager.Instance.Player;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == GameManager.Instance.Player.transform)       //싱글턴에서 생성한 객체이므로 단 하나뿐이다. 즉 비교가능
        {
            target = null;
        }
    }

    private void LookTarget() 
    {
        bool isFireStart = false;
        if (target != null)
        {                                                                                                //body의 벡터를 dir로 교체
            Vector3 dir = target.transform.position - transform.position;   //목표까지의 방향벡터 설정     //body.forward = dir->즉발
            dir.y = 0.0f;                                               //목표벡터까지의 각도 구함
            body.rotation = Quaternion.Slerp(body.rotation, Quaternion.LookRotation(dir), Time.deltaTime * turnSpeed);
            //각도계산 - 두벡터간 가장 작은 사이각 반환 |SignedAngle-두벡터간 사이각을 방향고려해 반환(음수개념 존재)
            float angle = Vector3.Angle(body.forward, dir);
            if (angle < fireAngle)
            {
                isFireStart = true;
            }
        }

        if (isFireStart)
        {
            StartFire();
        }
        else 
        {
            StopFire();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, transform.up, sightRange, 0.3f);
        if (body == null) 
        {
            body = transform.GetChild(2);
        }

        Vector3 from = transform.position;
        Vector3 to = transform.position + body.transform.forward * sightRange;
        Gizmos.color = Color.yellow;
        Handles.DrawDottedLine(from, to, 2.0f);

        Handles.color = Color.red;
        Vector3 dir1 = Quaternion.AngleAxis(-fireAngle, transform.up) * body.forward;
        Vector3 dir2 = Quaternion.AngleAxis(fireAngle, transform.up) * body.forward;

        to= transform.position + dir1 * sightRange;
        Handles.DrawLine(from, to);
        to = transform.position + dir2 * sightRange;
        Handles.DrawLine(from, to);
    }
#endif
}

///추적용터렛만들기
///플레이어가 일정거리안 접근시 플레이어 방향으로 몸체 회전(y축만)
///플레이어가 터렛의 발사각안에 있을시 총알을 주기적으로 발사 벗어나면 사격중지
///기즈모사용으로 시야범위와 발사각 그리기(Handles 추천)
