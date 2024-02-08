using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WayPointUser : MonoBehaviour
{
    public WayPoints targetWaypoints;
    public float moveSpeed = 5.0f;

    Transform target;
    Vector3 moveDirection;

    protected Vector3 moveDelta = Vector3.zero;             //이번 물리 프레임에서 움직인 양
    protected virtual Transform Target 
    {
        get => target;
        set 
        {
            target = value;
            moveDirection = (target.position - transform.position).normalized;
        }
    }

    bool IsArrived                                         //도착지점에 근접했는가 [벡터3는 == 계산 하지 마라]
    {
        get 
        {
            return (target.position - transform.position).sqrMagnitude < 0.01f;
        }
    }

    private void Start()
    {
        Target = targetWaypoints.currentWayPoint;
    }

    private void FixedUpdate()
    {
        OnMove();
    }

    protected virtual void OnMove() 
    {
        moveDelta = Time.fixedDeltaTime * moveSpeed * moveDirection;
        transform.Translate(moveDelta, Space.World);                //본체도 움직이기 때문에 로컬좌표계로하면 움직임이 뒤틀림
        if (IsArrived)                                              //MoveTowards는 정확하나 연산량이 많다.
        {
            OnArrived();
        }
    }

    protected virtual void OnArrived()
    {
        Target = targetWaypoints.GetNextPoint();
    }


}
