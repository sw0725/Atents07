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

    protected Vector3 moveDelta = Vector3.zero;             //�̹� ���� �����ӿ��� ������ ��
    protected virtual Transform Target 
    {
        get => target;
        set 
        {
            target = value;
            moveDirection = (target.position - transform.position).normalized;
        }
    }

    bool IsArrived                                         //���������� �����ߴ°� [����3�� == ��� ���� ����]
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
        transform.Translate(moveDelta, Space.World);                //��ü�� �����̱� ������ ������ǥ����ϸ� �������� ��Ʋ��
        if (IsArrived)                                              //MoveTowards�� ��Ȯ�ϳ� ���귮�� ����.
        {
            OnArrived();
        }
    }

    protected virtual void OnArrived()
    {
        Target = targetWaypoints.GetNextPoint();
    }


}
