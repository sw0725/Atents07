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
#if UNITY_EDITOR
    bool IsRedState => isFire;
    bool IsOrengeState => IsTargetVisible;
    bool IsTargetVisible = false;
#endif

    SphereCollider sightTrigger;
    Player target;

    protected override void Awake()
    {
        base.Awake();
        sightTrigger = GetComponent<SphereCollider>();
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
        if (other.transform == GameManager.Instance.Player.transform) 
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

            if (isVisibleTarget(dir))
            {
                body.rotation = Quaternion.Slerp(body.rotation, Quaternion.LookRotation(dir), Time.deltaTime * turnSpeed);
                //각도계산 - 두벡터간 가장 작은 사이각 반환 |SignedAngle-두벡터간 사이각을 방향고려해 반환(음수개념 존재)
                float angle = Vector3.Angle(body.forward, dir);
                if (angle < fireAngle)
                {
                    isFireStart = true;
                }
            }

        }
#if UNITY_EDITOR
        else 
        {
            IsTargetVisible = false;
        }
#endif

        if (isFireStart)
        {
            StartFire();
        }
        else 
        {
            StopFire();
        }
    }

    protected void StartFire()
    {
        if (!isFire)
        {
            StartCoroutine(FireCoroutine);
            isFire = true;
        }
    }

    protected void StopFire()
    {
        if (isFire)
        {
            StopCoroutine(FireCoroutine);
            isFire = false;
        }
    }

    private bool isVisibleTarget(Vector3 lookDirection) 
    {
        bool result = false;
                                                            //out=출력전용 -> 이런파라미터는 초기화 할 필요없음 함수실행시 값이 자동 삭제됨 
        Ray ray = new Ray(body.position, lookDirection);    //ref=출력(참조)-> 참조값을 파라미터로 받음 해당값 변형시 원본 변경됨
        if (Physics.Raycast(ray, out RaycastHit hitInfo, sightRange))  
        {                                                   //레이어에서 총알의 레이어를 Ignore Raycast로 변경->총알이 레이 막는 문제 예방
            if (hitInfo.transform == target.transform)      //혹은 LayerMask.GetMask("Player") 를 통해 특정 레이어만 감지하게 할 수 있다
            {
                result = true;
            }
        }
#if UNITY_EDITOR
        IsTargetVisible = result;
#endif
        return result;
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Handles.DrawWireDisc(transform.position, transform.up, sightRange, 0.3f);
        if (body == null) 
        {
            body = transform.GetChild(2);
        }

        Vector3 from = transform.position;
        Vector3 to = transform.position + body.transform.forward * sightRange;
        Gizmos.color = Color.yellow;
        Handles.DrawDottedLine(from, to, 2.0f);

        Handles.color = Color.green;
        if (IsRedState)
        {
            Handles.color = Color.red;
        }
        else if (IsOrengeState) 
        {                                                       //스택메모리-스테틱같은
            Handles.color = new Color(1.0f, 0.5f, 0.0f); //컬러는 값타입 얼마든지 뉴해도 상관없다
        }

        Vector3 dir1 = Quaternion.AngleAxis(-fireAngle, transform.up) * body.forward;
        Vector3 dir2 = Quaternion.AngleAxis(fireAngle, transform.up) * body.forward;

        to= transform.position + dir1 * sightRange;
        Handles.DrawLine(from, to);
        to = transform.position + dir2 * sightRange;
        Handles.DrawLine(from, to);
        Handles.DrawWireArc(from, transform.up, dir1, fireAngle * 2.0f, sightRange);
    }               //원중앙, 그리는 방향, 시작각, 시작각에서 종료각, 반지름
#endif
}

///추적용터렛만들기
///플레이어가 일정거리안 접근시 플레이어 방향으로 몸체 회전(y축만)
///플레이어가 터렛의 발사각안에 있을시 총알을 주기적으로 발사 벗어나면 사격중지
///기즈모사용으로 시야범위와 발사각 그리기(Handles 추천)
