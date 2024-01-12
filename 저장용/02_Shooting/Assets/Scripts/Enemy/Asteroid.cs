using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Asteroid : RecycleObject
{
    Vector3 direction = Vector3.zero;

    public float moveSpeed = 3.0f;
    public float rotateSpeed = 360.0f;

    public void SetDestination(Vector3 destination) 
    {
        direction = (destination - transform.position).normalized;
    }

    private void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime*rotateSpeed);
        transform.Translate(Time.deltaTime * moveSpeed * direction, Space.World);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + direction);
    }
}

//���������� ȸ��, ������ƮǮ, ������, ����,���� ����, ������ �������ϰ�
